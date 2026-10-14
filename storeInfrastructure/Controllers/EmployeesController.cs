using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using storeDomain.Interfaces;
using System.Security.Claims;

namespace storeInfrastructure.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeesController : Controller
{
    private readonly IStoreUnitOfWork _unitOfWork;

    public EmployeesController(IStoreUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var employees = await _unitOfWork.Employees.GetAllAsync(cancellationToken);
        return View(employees);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Fire(int employeeId, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int adminUserId))
        {
            return Forbid();
        }

        var transactionStarted = false;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            transactionStarted = true;

            await _unitOfWork.Employees.FireEmployeeAsync(employeeId, adminUserId, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            transactionStarted = false;

            TempData["SuccessMessage"] = $"Employee #{employeeId} was dismissed and user account deactivated";
        }
        catch (PostgresException ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore */ }
            }
            TempData["ErrorMessage"] = ex.MessageText;
        }
        catch (Exception ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore */ }
            }
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}