namespace storeDomain.Interfaces;

public interface IStoreUnitOfWork : IAsyncDisposable
{
    IInventoryRepository Inventory { get; }
    ISalesRepository Sales { get; }
    IEmployeeRepository Employees { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}