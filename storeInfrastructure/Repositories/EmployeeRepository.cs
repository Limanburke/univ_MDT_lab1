using Microsoft.EntityFrameworkCore;
using storeDomain.Interfaces;
using storeDomain.Model;

namespace storeInfrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly StoreDbContext _context;

    public EmployeeRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<VEmployeeDirectory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VEmployeeDirectories
        .AsNoTracking()
        .OrderBy(e => e.IsDeleted)
        .ThenBy(e => e.FullName)
        .ToListAsync(cancellationToken);
    }

    public async Task FireEmployeeAsync(int employeeId, int adminUserId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_fire_employee({employeeId}, {adminUserId})",
            cancellationToken);
    }
}