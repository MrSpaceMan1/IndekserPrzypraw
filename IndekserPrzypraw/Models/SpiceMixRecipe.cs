namespace IndekserPrzypraw.Models;

public class SpiceMixRecipe
{
  public int SpiceMixRecipeId { get; set; }
  public string Name { get; set; }
  public ICollection<Ingredient> Ingredients { get; set; }  
}