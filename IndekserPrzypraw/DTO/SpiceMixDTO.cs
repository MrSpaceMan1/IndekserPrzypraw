namespace IndekserPrzypraw.DTO;

public class SpiceMixDTO
{
  public int SpiceMixRecipeId { get; set; }
  public string Name { get; set; }
  public List<IngredientDTO> Ingredients { get; set; }
}