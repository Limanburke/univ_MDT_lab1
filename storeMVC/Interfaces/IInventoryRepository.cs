using storeDomain.Model;

namespace storeDomain.Interfaces;

public interface IInventoryRepository
{
    Task<IReadOnlyList<VDepartmentInventory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VActiveProduct>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VUserAuditLog>> GetAuditLogsAsync(CancellationToken cancellationToken = default);
    Task AddStockAsync(int departmentId, int productId, int quantity, int? userId, CancellationToken cancellationToken = default);
}