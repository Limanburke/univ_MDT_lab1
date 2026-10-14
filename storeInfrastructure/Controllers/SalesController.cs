using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;
using storeDomain.Interfaces;
using storeInfrastructure.Models;
using System.Security.Claims;

namespace storeInfrastructure.Controllers;

[Authorize]
public class SalesController : Controller
{
    private readonly IStoreUnitOfWork _unitOfWork;
    public SalesController(IStoreUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var sales = await _unitOfWork.Sales.GetSummaryAsync(cancellationToken);
            return View(sales);
        }
        catch (OperationCanceledException)
        {
            return new EmptyResult();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new CreateSaleViewModel();
        await PopulateDropdownsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Cashier")]
    public async Task<IActionResult> Create(CreateSaleViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model, cancellationToken);
            return View(model);
        }

        var transactionStarted = false;
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            transactionStarted = true;

            int? currentUserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int uid) ? uid : null;

            await _unitOfWork.Sales.CreateSaleAsync(
                model.DepartmentId,
                model.EmployeeId,
                model.ProductId,
                model.Quantity,
                model.CustomerId,
                currentUserId,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            transactionStarted = false;

            TempData["SuccessMessage"] = "Sale completed successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (PostgresException ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore secondary error */ }
            }
            TempData["ErrorMessage"] = ex.MessageText;
        }
        catch (Exception ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore secondary error */ }
            }
            TempData["ErrorMessage"] = ex.Message;
        }

        await PopulateDropdownsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int transactionId, CancellationToken cancellationToken)
    {
        int adminUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transactionStarted = false;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            transactionStarted = true;

            await _unitOfWork.Sales.CancelReceiptAsync(transactionId, adminUserId, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            transactionStarted = false;

            TempData["SuccessMessage"] = $"Receipt #{transactionId} was successfully cancelled.";
        }
        catch (PostgresException ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore secondary error */ }
            }
            TempData["ErrorMessage"] = ex.MessageText;
        }
        catch (Exception ex)
        {
            if (transactionStarted)
            {
                try { await _unitOfWork.RollbackAsync(CancellationToken.None); } catch { /* ignore secondary error */ }
            }
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(CreateSaleViewModel model, CancellationToken cancellationToken)
    {
        var departments = await _unitOfWork.Sales.GetActiveDepartmentsAsync(cancellationToken);
        model.Departments = departments
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
            .ToList();

        var employees = await _unitOfWork.Sales.GetActiveEmployeesAsync(cancellationToken);
        model.Employees = employees
            .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.FullName })
            .ToList();

        var activeProducts = await _unitOfWork.Inventory.GetActiveProductsAsync(cancellationToken);
        model.Products = activeProducts
            .Where(p => p.ProductId.HasValue)
            .Select(p => new SelectListItem
            {
                Value = p.ProductId.Value.ToString(),
                Text = $"{p.ProductName} (SKU: {p.Sku}, Price: {p.Price:0.00}, Cat: {p.CategoryName})"
            })
            .ToList();

        var customers = await _unitOfWork.Sales.GetActiveCustomersAsync(cancellationToken);
        var customerItems = customers
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.FullName} ({c.Phone})" })
            .ToList();

        customerItems.Insert(0, new SelectListItem { Value = "", Text = "-- Anonymous Customer --" });
        model.Customers = customerItems;
    }
}