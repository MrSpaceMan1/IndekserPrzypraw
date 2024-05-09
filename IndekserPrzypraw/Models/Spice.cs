using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.Models;

public class Spice
{
  public int SpiceId { get; set; }
  [Required] public int SpiceGroupId { get; set; }
  public SpiceGroup SpiceGroup { get; set; }
  public DateOnly? ExpirationDate { get; set; }

  public override string ToString()
  {
    return $"Spice({nameof(SpiceId)}: {SpiceId}, {nameof(SpiceGroupId)}: {SpiceGroupId}, " +
           $" {nameof(ExpirationDate)}: {ExpirationDate})";
  }
}