using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzyprawTests.Mocks;

public class UnitOfWorkStub<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
  public TContext Context { get; }

  public async Task BeginTransaction()
  {
  }

  public async Task Rollback()
  {
  }

  public async Task Commit()
  {
  }

  public async Task Save()
  {
  }
}