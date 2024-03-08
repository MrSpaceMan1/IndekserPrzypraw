using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceGroupRepository
{
  Task<IEnumerable<SpiceGroup>> GetAllSpiceGroupsAsync();
  Task<SpiceGroup?> GetSpiceGroupByIdAsync(int spiceGroupId);
  Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName);
  Task<SpiceGroup> AddSpiceGroupAsync(string name, uint? minimumCount, uint? minimumGrams);
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
  
  public async Task<IEnumerable<SpiceGroup>> GetAllSpiceGroupsAsync()
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .ToListAsync();
  }

  public async Task<SpiceGroup?> GetSpiceGroupByIdAsync(int spiceGroupId)
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.SpiceGroupId == spiceGroupId)
      .FirstOrDefaultAsync();
  }

  public async Task<SpiceGroup?> GetSpiceGroupByNameAsync(string spiceGroupName)
  {
    return await _context.SpiceGroups
      .AsNoTracking()
      .Where(group => group.Name == spiceGroupName)
      .FirstOrDefaultAsync();
  }

  public async Task<SpiceGroup> AddSpiceGroupAsync(string name, uint? minimumCount, uint? minimumGrams)
  {
    SpiceGroup newSpiceGroup = new SpiceGroup
    {
      Name = name,
      MinimumCount = minimumCount,
      MinimumGrams = minimumGrams
    };

    _context.SpiceGroups.Add(newSpiceGroup);
    await _context.SaveChangesAsync();
    return newSpiceGroup; 
  }
}