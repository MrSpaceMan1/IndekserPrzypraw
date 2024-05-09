using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceGroupRepository
{
  Task<SpiceGroup?> GetSpiceByBarcodeAsync(string barcode);
  Task<IEnumerable<SpiceGroup>> GetAllSpiceGroupsAsync();
  Task<SpiceGroup?> GetSpiceGroupByIdAsync(int spiceGroupId);
  Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName, int drawerId);

  Task<SpiceGroup> AddSpiceGroupAsync(string name, string barcode,
    int grams, int drawerId, int? minimumCount, int? minimumGrams);

  Task RemoveSpiceGroupAsync(int spiceGroupId);
  Task TransferSpiceGroupsAsync(int fromDrawerId, int toDrawerId);
  Task<SpiceGroup> UpdateSpiceGroupAsync(SpiceGroup spiceGroup);
}

public class SpiceGroupRepository : ISpiceGroupRepository
{
  private readonly SpicesContext _context;
  private readonly UnitOfWork<SpicesContext> _unitOfWork;

  public SpiceGroupRepository(UnitOfWork<SpicesContext> unitOfWork)
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

  public async Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName, int drawerId)
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.Name == spiceGroupName)
      .Where(group => group.DrawerId == drawerId)
      .Include(spiceGroup => spiceGroup.Spices)
      .FirstOrDefaultAsync();
  }

  public async Task<SpiceGroup> AddSpiceGroupAsync(string name, string barcode,
    int grams, int drawerId, int? minimumCount, int? minimumGrams)
  {
    SpiceGroup newSpiceGroup = new SpiceGroup
    {
      Name = name,
      Grams = grams,
      MinimumCount = minimumCount,
      MinimumGrams = minimumGrams,
      Barcode = barcode,
      DrawerId = drawerId
    };

    _context.SpiceGroups.Add(newSpiceGroup);
    await _context.SaveChangesAsync();
    return newSpiceGroup;
  }

  public async Task TransferSpiceGroupsAsync(int fromDrawerId, int toDrawerId)
  {
    var spiceGroups = await _context.SpiceGroups
      .Where(spiceGroup => spiceGroup.SpiceGroupId == fromDrawerId)
      .ToListAsync();
    if (spiceGroups.IsNullOrEmpty())
      throw new NotFoundException(opt =>
        opt.AddModelError(nameof(fromDrawerId), "Spice group with provided id doesn't exist"));

    foreach (var group in spiceGroups)
    {
      group.DrawerId = toDrawerId;
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