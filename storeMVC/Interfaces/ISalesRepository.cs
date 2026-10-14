using storeDomain.Model;

namespace storeDomain.Interfaces;

public interface ISalesRepository
{
    Task<IReadOnlyList<VSalesSummary>> GetSummaryAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VActiveDepartment>> GetActiveDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VActiveEmployee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VActiveCustomer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task CancelReceiptAsync(int transactionId, int adminUserId, CancellationToken cancellationToken = default);
    Task CreateSaleAsync(int departmentId, int employeeId, int productId, int quantity, int? customerId, int? userId, CancellationToken cancellationToken = default);
}