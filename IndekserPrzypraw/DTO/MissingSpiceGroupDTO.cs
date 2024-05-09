namespace IndekserPrzypraw.DTO;

public class MissingSpiceGroupDTO
{
  public string Name { get; set; }
  public int SpiceGroupId { get; set; }
  public int MissingGrams { get; set; }
  public int MissingCount { get; set; }
}