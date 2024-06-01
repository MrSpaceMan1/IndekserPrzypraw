using IndekserPrzypraw.Models;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzyprawTests.Mocks;

public class SpiceRepositoryFake : ISpiceRepository
{
  private List<Spice> _inMemorySpices = [];

  public Task<Spice> AddSpiceAsync(Spice spice)
  {
    _inMemorySpices.Add(spice);
    return Task.FromResult(spice);
  }

  public Task<IEnumerable<Spice>> GetAllSpicesAsync()
  {
    return Task.FromResult(_inMemorySpices.AsEnumerable());
  }

  public Task<IEnumerable<Spice>> GetAllSpicesFromDrawerAsync(int drawerId)
  {
    return Task.FromResult(_inMemorySpices.Where(spice => spice.SpiceGroup.DrawerId == drawerId));
  }

  public Task<Spice?> GetSpiceByIdAsync(int id)
  {
    return Task.FromResult(_inMemorySpices.Find(spice => spice.SpiceId == id));
  }

  public Task DeleteSpiceAsync(Spice spice)
  {
    _inMemorySpices.Remove(spice);
    return Task.CompletedTask;
  }
}