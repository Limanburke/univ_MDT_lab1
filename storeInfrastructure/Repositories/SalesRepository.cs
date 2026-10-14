using Microsoft.EntityFrameworkCore;
using storeDomain.Interfaces;
using storeDomain.Model;

namespace storeInfrastructure.Repositories;

public class SalesRepository : ISalesRepository
{
    private readonly StoreDbContext _context;

    public SalesRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<VSalesSummary>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VSalesSummaries
            .AsNoTracking()
            .OrderByDescending(s => s.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VActiveDepartment>> GetActiveDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VActiveDepartments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VActiveEmployee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VActiveEmployees
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VActiveCustomer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VActiveCustomers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task CancelReceiptAsync(int transactionId, int adminUserId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_cancel_receipt({transactionId}, {adminUserId})",
            cancellationToken);
    }

    public async Task CreateSaleAsync(
    int departmentId,
    int employeeId,
    int productId,
    int quantity,
    int? customerId,
    int? userId,
    CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_create_sale({departmentId}, {employeeId}, {productId}, {quantity}, CAST({customerId} AS integer), CAST({userId} AS integer))",
            cancellationToken);
    }
}