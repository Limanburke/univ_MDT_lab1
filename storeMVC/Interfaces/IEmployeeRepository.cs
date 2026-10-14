using storeDomain.Model;

namespace storeDomain.Interfaces;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<VEmployeeDirectory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task FireEmployeeAsync(int employeeId, int adminUserId, CancellationToken cancellationToken = default);
}