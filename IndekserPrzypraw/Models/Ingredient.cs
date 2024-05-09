namespace IndekserPrzypraw.Models;

public class Ingredient
{
  public int IngredientId { get; set; }
  public SpiceGroup SpiceGroup { get; set; }
  public uint Grams { get; set; }
}