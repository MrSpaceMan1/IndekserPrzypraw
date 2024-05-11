using IndekserPrzypraw.Models;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceMixRepository
{
  public Task<List<SpiceMixRecipe>> GetAllSpiceMixAsync();
  public Task<SpiceMixRecipe?> GetSpiceMixAsync(int id);
  public Task<int> AddSpiceMixAsync(SpiceMixRecipe spiceMixToAdd);
  public Task<int> ChangeSpiceMixAsync(SpiceMixRecipe changes);
  public Task<int> RemoveSpiceMixAsync(int id);
}

public class SpiceMixRepository : ISpiceMixRepository
{
  private SpicesContext _spiceContext;

  public SpiceMixRepository(SpicesContext spicesContext)
  {
    _spiceContext = spicesContext;
  }

  public Task<List<SpiceMixRecipe>> GetAllSpiceMixAsync()
  {
    return _spiceContext.SpiceMixRecipes
      .AsNoTracking()
      .Include(mix => mix.Ingredients)
      .ThenInclude(ingredient => ingredient.SpiceGroup)
      .ToListAsync();
  }

  public Task<SpiceMixRecipe?> GetSpiceMixAsync(int id)
  {
    return _spiceContext.SpiceMixRecipes
      .AsNoTracking()
      .Include(mix => mix.Ingredients)
      .ThenInclude(ingredient => ingredient.SpiceGroup)
      .FirstOrDefaultAsync(mix => mix.SpiceMixRecipeId == id);
  }

  public Task<int> AddSpiceMixAsync(SpiceMixRecipe spiceMixToAdd)
  {
    _spiceContext.SpiceMixRecipes.Add(spiceMixToAdd);
    return _spiceContext.SaveChangesAsync();
  }

  public Task<int> ChangeSpiceMixAsync(SpiceMixRecipe changes)
  {
    _spiceContext.SpiceMixRecipes.Update(changes);
    return _spiceContext.SaveChangesAsync();
  }

  public async Task<int> RemoveSpiceMixAsync(int id)
  {
    await _spiceContext.SpiceMixRecipes.FirstAsync().ContinueWith(_spiceContext.Remove);
    return await _spiceContext.SaveChangesAsync();
  }
}