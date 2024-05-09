namespace IndekserPrzypraw.DTO;

public class MissingSpicesDTO
{
  public Dictionary<string, ICollection<MissingSpiceGroupDTO>> MissingSpices { get; set; }
}