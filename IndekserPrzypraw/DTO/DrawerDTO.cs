namespace IndekserPrzypraw.DTO;

public class DrawerDTO
{
  public int DrawerId { get; set; }
  public string Name { get; set; }
  public List<SpiceGroupDTO> Spices { get; set; } = [];

  public override string ToString()
  {
    return $"{nameof(DrawerId)}: {DrawerId}, {nameof(Name)}: {Name}, {nameof(Spices)}: {Spices}";
  }
}