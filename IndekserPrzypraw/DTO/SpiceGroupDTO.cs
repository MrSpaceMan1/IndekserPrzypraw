namespace IndekserPrzypraw.DTO;

public class SpiceGroupDTO
{
  public int SpiceGroupId { get; set; }
  public string Name { get; set; }
  public int? MinimumCount { get; set; }
  public int? MinimumGrams { get; set; }
  public List<SpiceDTO> Spices { get; set; }

  public override string ToString()
  {
    return
      $"{nameof(SpiceGroupId)}: {SpiceGroupId}, {nameof(Name)}: {Name}, {nameof(MinimumCount)}: {MinimumCount}, " +
      $"{nameof(MinimumGrams)}: {MinimumGrams}";
  }
}