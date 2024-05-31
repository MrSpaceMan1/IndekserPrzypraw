using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Repositories;

public interface ISpiceMixRepository
{
  public Task<List<SpiceMixRecipe>> GetAllSpiceMixAsync();
  public Task<SpiceMixRecipe?> GetSpiceMixAsync(int id);
  public Task<SpiceMixRecipe> AddSpiceMixAsync(SpiceMixRecipe spiceMixToAdd);
  public Task<SpiceMixRecipe> ChangeSpiceMixAsync(SpiceMixRecipe changes);
  public Task<int> RemoveSpiceMixAsync(int id);

  public Task<IEnumerable<Ingredient>> AddIngredientsAsync(IEnumerable<Ingredient> ingredients);
  public Task RemoveIngredientsAsync(int spiceMixId);
}

public class SpiceMixRepository : ISpiceMixRepository
{
  private SpicesContext _spiceContext;

  public SpiceMixRepository(IUnitOfWork<SpicesContext> unitOfWork)
  {
    _spiceContext = unitOfWork.Context;
  }

  public Task<List<SpiceMixRecipe>> GetAllSpiceMixAsync()
  {
    return _spiceContext.SpiceMixRecipes
      .AsNoTracking()
      .Include(mix => mix.Ingredients)
      .ToListAsync();
  }

  public Task<SpiceMixRecipe?> GetSpiceMixAsync(int id)
  {
    return _spiceContext.SpiceMixRecipes
      .AsNoTracking()
      .Include(mix => mix.Ingredients)
      .FirstOrDefaultAsync(mix => mix.SpiceMixRecipeId == id);
  }

  public async Task<SpiceMixRecipe> AddSpiceMixAsync(SpiceMixRecipe spiceMixToAdd)
  {
    _spiceContext.SpiceMixRecipes.Add(spiceMixToAdd);
    await _spiceContext.SaveChangesAsync();
    return spiceMixToAdd;
  }

  public async Task<SpiceMixRecipe> ChangeSpiceMixAsync(SpiceMixRecipe changes)
  {
    _spiceContext.SpiceMixRecipes.Update(changes);
    await _spiceContext.SaveChangesAsync();
    return changes;
  }

  public async Task<int> RemoveSpiceMixAsync(int id)
  {
    var spiceMix = await _spiceContext.SpiceMixRecipes.FirstOrDefaultAsync(mix => mix.SpiceMixRecipeId == id);
    if (spiceMix is null)
      throw new NotFoundException(x => x.AddModelError(nameof(id), $"SpiceMix with id {id} doesn't exist"));
    _spiceContext.Remove(spiceMix);
    return await _spiceContext.SaveChangesAsync();
  }

  public async Task<IEnumerable<Ingredient>> AddIngredientsAsync(IEnumerable<Ingredient> ingredients)
  {
    var addIngredientsAsync = ingredients.ToList();
    _spiceContext.AddRange(addIngredientsAsync);
    await _spiceContext.SaveChangesAsync();
    return addIngredientsAsync;
  }

  public async Task RemoveIngredientsAsync(int spiceMixId)
  {
    var spiceMix =  await _spiceContext.SpiceMixRecipes.Include(mix => mix.Ingredients).FirstOrDefaultAsync(mix => mix.SpiceMixRecipeId == spiceMixId);
    if (spiceMix is null)
      throw new NotFoundException(x =>
        x.AddModelError(nameof(spiceMixId), $"SpiceMix with id {spiceMixId} doesn't exist"));
    _spiceContext.RemoveRange(spiceMix.Ingredients);
    await _spiceContext.SaveChangesAsync();
  }
}