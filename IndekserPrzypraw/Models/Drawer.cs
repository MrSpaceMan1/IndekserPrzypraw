using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.Models;

public class Drawer
{
  public int DrawerId { get; set; }
  public string Name { get; set; }
  public ICollection<Spice> Spices { get; set; } = [];

  public override string ToString()
  {
    return $"{nameof(DrawerId)}: {DrawerId}, {nameof(Name)}: {Name}, {nameof(Spices)}: [{string.Join(",", Spices)}]";
  }
}