using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceGroupRepository
{
  Task<SpiceGroup?> GetSpiceByBarcodeAsync(string barcode);
  Task<IEnumerable<SpiceGroup>> GetAllSpiceGroupsAsync();
  Task<SpiceGroup?> GetSpiceGroupByIdAsync(int spiceGroupId);
  Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName, int drawerId);

  Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName);
  Task<SpiceGroup> AddSpiceGroupAsync(SpiceGroup spiceGroup);

  Task RemoveSpiceGroupAsync(int spiceGroupId);
  Task TransferSpiceGroupsAsync(int fromDrawerId, int toDrawerId);
  Task<SpiceGroup> UpdateSpiceGroupAsync(SpiceGroup spiceGroup);
}

public class SpiceGroupRepository : ISpiceGroupRepository
{
  private readonly SpicesContext _context;
  private readonly IUnitOfWork<SpicesContext> _unitOfWork;

  public SpiceGroupRepository(IUnitOfWork<SpicesContext> unitOfWork)
  {
    _unitOfWork = unitOfWork;
    _context = unitOfWork.Context;
  }

  public async Task<SpiceGroup?> GetSpiceByBarcodeAsync(string barcode)
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.Barcode == barcode)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<SpiceGroup>> GetAllSpiceGroupsAsync()
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .Include(spiceGroup => spiceGroup.Spices)
      .ToListAsync();
  }

  public Task<SpiceGroup?> GetSpiceGroupByIdAsync(int spiceGroupId)
  {
    return _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.SpiceGroupId == spiceGroupId)
      .Include(spiceGroup => spiceGroup.Spices)
      .FirstOrDefaultAsync();
  }

  public Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName, int drawerId)
  {
    return _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.Name == spiceGroupName)
      .Where(group => group.DrawerId == drawerId)
      .Include(spiceGroup => spiceGroup.Spices)
      .FirstOrDefaultAsync();
  }

  public Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName)
  {
    return _context.SpiceGroups.AsNoTracking().FirstOrDefaultAsync(group => group.Name == spiceGroupName);
  }

  public async Task<SpiceGroup> AddSpiceGroupAsync(SpiceGroup spiceGroup)
  {
    _context.SpiceGroups.Add(spiceGroup);
    await _context.SaveChangesAsync();
    return spiceGroup;
  }

  public async Task TransferSpiceGroupsAsync(int fromDrawerId, int toDrawerId)
  {
    IEqualityComparer<SpiceGroup> comparer =
      new ValueComparer<SpiceGroup>((group1, group2) => group1.Name == group2.Name, group => group.Name.GetHashCode());

    var fromDrawerSpiceGroups = (await _context.SpiceGroups
      .Include(g => g.Spices)
      .Where(spiceGroup => spiceGroup.DrawerId == fromDrawerId)
      .ToListAsync());

    if (fromDrawerSpiceGroups.IsNullOrEmpty())
      throw new NotFoundException(opt =>
        opt.AddModelError(nameof(fromDrawerId), "Spice group with provided id doesn't exist"));

    var toDrawerSpiceGroups =
      (await _context.SpiceGroups.Where(group => group.DrawerId == toDrawerId).ToListAsync()).ToHashSet(comparer);
    foreach (var fromDrawerSpiceGroup in fromDrawerSpiceGroups)
    {
      SpiceGroup? foundSpiceGroup;
      if (toDrawerSpiceGroups.TryGetValue(fromDrawerSpiceGroup, out foundSpiceGroup))
      {
        _context.Spices.UpdateRange(fromDrawerSpiceGroup.Spices.Select(group =>
        {
          group.SpiceGroupId = foundSpiceGroup.SpiceGroupId;
          return group;
        }));
        _context.SpiceGroups.Remove(fromDrawerSpiceGroup);
      }
      else
      {
        fromDrawerSpiceGroup.DrawerId = toDrawerId;
        _context.Update(fromDrawerSpiceGroup);
      }
    }

    await _context.SaveChangesAsync();
  }

  public async Task<SpiceGroup> UpdateSpiceGroupAsync(SpiceGroup spiceGroup)
  {
    _context.SpiceGroups.Update(spiceGroup);
    await _context.SaveChangesAsync();
    return spiceGroup;
  }

  public async Task RemoveSpiceGroupAsync(int spiceGroupId)
  {
    var spiceGroup =
      await _context.SpiceGroups.FirstOrDefaultAsync(spiceGroup => spiceGroup.SpiceGroupId == spiceGroupId);
    if (spiceGroup is null)
      throw new NotFoundException(x =>
        x.AddModelError($"{nameof(spiceGroupId)}", $"Spice group with id {spiceGroupId} doesn't exist"));
    _context.SpiceGroups.Remove(spiceGroup);
    await _context.SaveChangesAsync();
  }
}