using System.ComponentModel.DataAnnotations;
using IndekserPrzypraw.Models;

namespace IndekserPrzypraw.Domain;

public class Spice
{
  public int SpiceId { get; set; }
  [Required]
  public int SpiceGroupId { get; set; }
  public virtual SpiceGroup SpiceGroup { get; set; }
  [Required]
  public int DrawerId { get; set; }
  public virtual Drawer Drawer { get; set; }
  public DateOnly ExpirationDate { get; set; }
  public uint Grams { get; set; }

  public override string ToString()
  {
    return $"{nameof(SpiceId)}: {SpiceId}, {nameof(SpiceGroupId)}: {SpiceGroupId}, {nameof(DrawerId)}: {DrawerId}, {nameof(ExpirationDate)}: {ExpirationDate}, {nameof(Grams)}: {Grams}";
  }
}
