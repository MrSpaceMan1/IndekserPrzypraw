using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using IndekserPrzypraw.Domain;

namespace IndekserPrzypraw.Models;

public class Drawer
{
  public int DrawerId { get; set; }
  [MinLength(3)]
  public string Name { get; set; }
  public virtual Collection<Spice> Spices { get; set; }

  public override string ToString()
  {
    return $"{nameof(DrawerId)}: {DrawerId}, {nameof(Name)}: {Name}";
  }
}