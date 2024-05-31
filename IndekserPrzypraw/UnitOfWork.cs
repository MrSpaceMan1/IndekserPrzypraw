using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IndekserPrzypraw.Profiles;

public interface IUnitOfWork<TContext> where TContext : DbContext
{
  TContext Context { get; }
  Task BeginTransaction();
  Task Rollback();
  Task Commit();
  Task Save();
}

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
  public TContext Context { get; }
  private IDbContextTransaction? _transaction;

  public UnitOfWork(TContext context)
  {
    Context = context;
  }

  public async Task BeginTransaction()
  {
    _transaction = await Context.Database.BeginTransactionAsync();
  }

  public async Task Rollback()
  {
    if (_transaction is not null)
    {
      await _transaction.RollbackAsync();
      await _transaction.DisposeAsync();
      _transaction = null;
    }
  }

  public async Task Commit()
  {
    if (_transaction is not null)
    {
      await _transaction.CommitAsync();
      await _transaction.DisposeAsync();
      _transaction = null;
    }
  }

  public async Task Save()
  {
    await Context.SaveChangesAsync();
  }
}