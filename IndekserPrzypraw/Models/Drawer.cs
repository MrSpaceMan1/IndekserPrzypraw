using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzypraw.Models;

public class Drawer
{
  public int DrawerId { get; set; }
  public string Name { get; set; }
  public Collection<SpiceGroup> SpiceGroups { get; set; }

  public override string ToString()
  {
    return
      $"{nameof(DrawerId)}: {DrawerId}, {nameof(Name)}: {Name}, {nameof(SpiceGroups)}: [{string.Join(",", SpiceGroups)}]";
  }
}