using Microsoft.EntityFrameworkCore;
using storeDomain.Interfaces;
using storeDomain.Model;

namespace storeInfrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly StoreDbContext _context;

    public InventoryRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<VDepartmentInventory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VDepartmentInventories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VUserAuditLog>> GetAuditLogsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VUserAuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VActiveProduct>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VActiveProducts
            .AsNoTracking()
            .OrderBy(p => p.ProductName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddStockAsync(int departmentId, int productId, int quantity, int? userId, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero");
        }

        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_add_stock({departmentId}, {productId}, {quantity}, {userId})",
            cancellationToken);
    }
}