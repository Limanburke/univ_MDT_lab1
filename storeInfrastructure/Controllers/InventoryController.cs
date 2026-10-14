using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using storeDomain.Interfaces;
using storeInfrastructure.Models;
using System.Security.Claims;

namespace storeInfrastructure.Controllers;

[Authorize]
public class InventoryController : Controller
{
    private readonly IStoreUnitOfWork _unitOfWork;

    public InventoryController(IStoreUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            var inventory = await _unitOfWork.Inventory.GetAllAsync(cancellationToken);
            return View(inventory);
        }
        catch (OperationCanceledException)
        {
            return new EmptyResult();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Products(CancellationToken cancellationToken)
    {
        try
        {
            var activeProducts = await _unitOfWork.Inventory.GetActiveProductsAsync(cancellationToken);
            return View(activeProducts);
        }
        catch (OperationCanceledException)
        {
            return new EmptyResult();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Audit(CancellationToken cancellationToken)
    {
        try
        {
            var auditLogs = await _unitOfWork.Inventory.GetAuditLogsAsync(cancellationToken);
            return View(auditLogs);
        }
        catch (OperationCanceledException)
        {
            return new EmptyResult();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AddStock(AddStockViewModel model, CancellationToken cancellationToken)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Quantity must be greater than zero";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _unitOfWork.Inventory.AddStockAsync(
                model.DepartmentId,
                model.ProductId,
                model.Quantity,
                userId,
                cancellationToken);

            TempData["SuccessMessage"] = "Stock updated successfully";
        }
        catch (PostgresException ex)
        {
            TempData["ErrorMessage"] = ex.MessageText;
        }


        return RedirectToAction(nameof(Index));
    }
}