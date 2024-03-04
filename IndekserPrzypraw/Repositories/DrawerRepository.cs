using IndekserPrzypraw.Domain;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface IDrawerRepository
{
  Task<Drawer?> GetDrawerByIdAsync(int drawerId);
  Task<IEnumerable<Drawer>> GetAllDrawersAsync();
  Task<Drawer> AddDrawerAsync(Drawer drawer);
  Task RemoveDrawerAsync(Drawer drawer);
  Task UpdateDrawerAsync(Drawer drawer);
}

public class DrawerRepository : IDrawerRepository
{
  private readonly SpicesContext _context;
  private readonly UnitOfWork<SpicesContext> _unitOfWork;

  public DrawerRepository(UnitOfWork<SpicesContext> unitOfWork)
  {
    _unitOfWork = unitOfWork;
    _context = unitOfWork.Context;
  }
  
  public async Task<Drawer?> GetDrawerByIdAsync(int drawerId)
  {
    return await _context.Drawers.FindAsync(drawerId);
  }

  public async Task<IEnumerable<Drawer>> GetAllDrawersAsync()
  {
    return await _context.Drawers.AsNoTracking().ToListAsync();
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