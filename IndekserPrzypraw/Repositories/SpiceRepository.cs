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
  Task DeleteSpiceAsync(Spice spice);
}

public class SpiceRepository : ISpiceRepository
{
  private readonly SpicesContext _context;
  private readonly IUnitOfWork<SpicesContext> _unitOfWork;


  public SpiceRepository(IUnitOfWork<SpicesContext> unitOfWork)
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
      .Where(spice => spice.SpiceGroup.DrawerId == drawerId)
      .ToListAsync();
    return spices;
  }

  public async Task<Spice?> GetSpiceByIdAsync(int id)
  {
    return await _context.Spices.AsNoTracking().FirstOrDefaultAsync(spice => spice.SpiceId == id);
  }

  public async Task DeleteSpiceAsync(Spice spice)
  {
    _context.Remove(spice);
    await _context.SaveChangesAsync();
  }
}