using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.Models;

public class SpiceGroup
{
  public int SpiceGroupId { get; set; }

  public string Barcode { get; set; }

  [Required] [Range(0, Int32.MaxValue)] public int Grams { get; set; }
  [Required] public string Name { get; set; }
  [Range(0, Int32.MaxValue)] public int? MinimumCount { get; set; }
  [Range(0, Int32.MaxValue)] public int? MinimumGrams { get; set; }

  public int DrawerId { get; set; }

  public Drawer Drawer { get; set; }

  public ICollection<Spice> Spices { get; set; }

  public override string ToString()
  {
    return $"{nameof(SpiceGroupId)}: {SpiceGroupId}, {nameof(Barcode)}: {Barcode}, {nameof(Grams)}: {Grams}, " +
           $"{nameof(Name)}: {Name}, {nameof(MinimumCount)}: {MinimumCount}, {nameof(MinimumGrams)}: {MinimumGrams}, " +
           $"{nameof(DrawerId)}: {DrawerId}, {nameof(Spices)}: {Spices}";
  }
}