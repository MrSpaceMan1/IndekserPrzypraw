using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.Models;

public class SpiceGroup
{
  public int SpiceGroupId { get; set; }
  [Required]
  public string Name { get; set; }
  public uint? MinimumCount { get; set; }
  public uint? MinimumGrams { get; set; }
}