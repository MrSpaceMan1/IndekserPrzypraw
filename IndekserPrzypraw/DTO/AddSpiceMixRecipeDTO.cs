namespace IndekserPrzypraw.DTO;

public class AddSpiceMixRecipeDTO
{
  public string Name { get; set; }
  public List<AddIngredientDTO> Ingredients { get; set; }
}