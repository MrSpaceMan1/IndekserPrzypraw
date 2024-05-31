using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface IDrawerRepository
{
  Task<Drawer?> GetDrawerByIdAsync(int drawerId);
  Task<List<Drawer>> GetAllDrawersAsync();
  Task<Drawer> AddDrawerAsync(Drawer drawer);
  Task RemoveDrawerAsync(Drawer drawer);
  Task UpdateDrawerAsync(Drawer drawer);
}

public class DrawerRepository : IDrawerRepository
{
  private readonly SpicesContext _context;
  private readonly IUnitOfWork<SpicesContext> _unitOfWork;

  public DrawerRepository(IUnitOfWork<SpicesContext> unitOfWork)
  {
    _unitOfWork = unitOfWork;
    _context = unitOfWork.Context;
  }
  
  public Task<Drawer?> GetDrawerByIdAsync(int drawerId)
  {
    return _context.Drawers
      .Include(drawer => drawer.SpiceGroups)
      .ThenInclude(spiceGroup => spiceGroup.Spices)
      .Where(drawer => drawer.DrawerId == drawerId)
      .FirstOrDefaultAsync();
  }

  public Task<List<Drawer>> GetAllDrawersAsync()
  {
    return _context.Drawers
      .AsNoTracking()
      .Include(drawer => drawer.SpiceGroups)
      .ThenInclude(spice => spice.Spices)
      .ToListAsync();
  }

  public async Task<Drawer> AddDrawerAsync(Drawer newDrawer)
  {
    var exists = await _context.Drawers.AsNoTracking().FirstOrDefaultAsync(drawer => drawer.Name == newDrawer.Name);
    if (exists is not null)
      throw new NotUniqueException(x =>
        x.AddModelError(nameof(newDrawer.Name),
        $"Drawer with name {newDrawer.Name} already exist")
      );
    _context.Add(newDrawer);
    await _context.SaveChangesAsync();

    return newDrawer;
  }

  public async Task RemoveDrawerAsync(Drawer drawer)
  {
    _context.Remove(drawer);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateDrawerAsync(Drawer drawer)
  {
    _context.Update(drawer);
    await _context.SaveChangesAsync();
  }
}