using Microsoft.EntityFrameworkCore.Storage;
using storeDomain.Interfaces;
using storeDomain.Model;

namespace storeInfrastructure.Repositories;

public class StoreUnitOfWork : IStoreUnitOfWork
{
    private readonly StoreDbContext _context;
    private IDbContextTransaction? _transaction;

    public IInventoryRepository Inventory { get; }
    public ISalesRepository Sales { get; }
    public IEmployeeRepository Employees { get; }

    public StoreUnitOfWork(
        StoreDbContext context,
        IInventoryRepository inventory,
        ISalesRepository sales,
        IEmployeeRepository employees)
    {
        _context = context;
        Inventory = inventory;
        Sales = sales;
        Employees = employees;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("The transaction has already started");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("There is no active transaction");
        }

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("There is no active transaction");
        }

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}