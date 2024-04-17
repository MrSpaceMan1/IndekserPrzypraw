using System.Collections;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceRepository
{
  Task<Spice> AddSpiceAsync(Spice spice);
  Task<IEnumerable<Spice>> GetAllSpicesAsync();
  Task<IEnumerable<Spice>> GetAllSpicesFromDrawerAsync(int drawerId);
  Task<Spice?> GetSpiceByIdAsync(int id);
  Task<Dictionary<String, List<Spice>>> GetSpiceByGroupsAsync(int drawerId);

  Task DeleteSpiceAsync(Spice spice);

  Task TransferSpicesAsync(int fromDrawerId, int toDrawerId);
}

public class SpiceRepository : ISpiceRepository
{
  private readonly SpicesContext _context;
  private readonly UnitOfWork<SpicesContext> _unitOfWork;
    
  
  public SpiceRepository(UnitOfWork<SpicesContext> unitOfWork)
  {
    _unitOfWork = unitOfWork;
    _context = unitOfWork.Context;
  }

  public async Task<Spice> AddSpiceAsync(Spice spice)
  {
    _context.Spices.Add(spice);
    await _context.SaveChangesAsync();
    return spice;
  }

  public async Task<IEnumerable<Spice>> GetAllSpicesAsync()
  {
    IEnumerable<Spice> spices = await _context.Spices
      .AsNoTracking()
      .Include(spice => spice.SpiceGroup)
      .ToListAsync();
    return spices;
  }

  public async Task<IEnumerable<Spice>> GetAllSpicesFromDrawerAsync(int drawerId)
  {
    IEnumerable<Spice> spices = await _context.Spices
      .AsNoTracking()
      .Include(spice => spice.SpiceGroup)
      .Where(spice => spice.DrawerId == drawerId)
      .ToListAsync();
    return spices;
  }

  public async Task<Spice?> GetSpiceByIdAsync(int id)
  {
    return await _context.Spices.AsNoTracking().FirstOrDefaultAsync(spice => spice.SpiceId == id);
  }

  public async Task<Dictionary<String, List<Spice>>> GetSpiceByGroupsAsync(int drawerId)
  {
    // Dictionary<String, IEnumerable<Spice>> spiceGroups;

      var spiceGroups = await _context.Spices
        .AsNoTracking()
        .Where(spice => spice.Drawer.DrawerId == drawerId)
        .GroupBy(spice => spice.SpiceGroup.Name)
        .ToDictionaryAsync(group => group.Key, group => group.ToList());
    
    return spiceGroups;
  }

  public class GroupByDrawerIdThenSpiceGroupName
  {
    public int DrawerId;
    public string Name;
    public List<Spice> Spices;
    public GroupByDrawerIdThenSpiceGroupName(int drawerId, string name, List<Spice> spices)
    {
      DrawerId = drawerId;
      Name = name;
      Spices = spices;
    }
  }
  
  public async  Task<List<GroupByDrawerIdThenSpiceGroupName>> GetSpiceByGroupsAsync()
  {
    List<GroupByDrawerIdThenSpiceGroupName>  spicesByDrawers = await _context.Spices
        .AsNoTracking()
        .GroupBy(spice => new { spice.DrawerId, spice.SpiceGroup.Name },
          (key, spices) => new GroupByDrawerIdThenSpiceGroupName(key.DrawerId, key.Name, spices.ToList()))
        .ToListAsync();
    
    return spicesByDrawers;
  }

  public async Task DeleteSpiceAsync(Spice spice)
  {
    _context.Remove(spice);
    await _context.SaveChangesAsync();
  }

  public async Task TransferSpicesAsync(int fromDrawerId, int toDrawerId)
  {
    var asyncSpices = _context.Spices.Where(spice => spice.DrawerId == fromDrawerId).AsAsyncEnumerable();
    await foreach (var spice in asyncSpices)
    {
      spice.DrawerId = toDrawerId;
    }

    await _context.SaveChangesAsync();
  }
}