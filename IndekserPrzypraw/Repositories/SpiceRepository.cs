using System.Collections;
using IndekserPrzypraw.Domain;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceRepository
{
  Task<Spice> AddSpice(Spice spice);
  Task<IEnumerable<Spice>> GetAllSpices();
  Task<IEnumerable<Spice>> GetAllSpicesFromDrawerAsync(int drawerId);
  Task<IEnumerable<IGrouping<string,Spice>>> GetSpiceGroupsFromDrawerAsync(int drawerId);
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

  public async Task<Spice> AddSpice(Spice spice)
  {
    _context.Spices.Add(spice);
    await _context.SaveChangesAsync();
    return spice;
  }

  public async Task<IEnumerable<Spice>> GetAllSpices()
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

  public async Task<IEnumerable<IGrouping<string,Spice>>> GetSpiceGroupsFromDrawerAsync(int drawerId)
  {
    ICollection<IGrouping<string,Spice>> spiceGroups;
    await using (_context)
    { 
      spiceGroups = await _context.Spices
        .AsNoTracking()
        .Where((spice => spice.Drawer.DrawerId == drawerId))
        .GroupBy((spice => spice.SpiceGroup.Name), (spice => spice))
        .ToListAsync();
    }
    return spiceGroups;
  }
}