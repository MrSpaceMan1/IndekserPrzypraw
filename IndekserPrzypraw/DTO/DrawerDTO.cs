namespace IndekserPrzypraw.DTO;

public class DrawerDTO
{
  public int DrawerId { get; set; }
  public string Name { get; set; }
  public List<SpiceGroupDTO> Spices { get; set; }

  public DrawerDTO(int drawerId,
    string name,
    List<SpiceGroupDTO> spices)
  {
    DrawerId = drawerId;
    Name = name;
    Spices = spices;
  }
}