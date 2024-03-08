using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace IndekserPrzypraw.Models;

public class SpicesContext : DbContext
{
  public DbSet<Drawer> Drawers { get; set; }
  public DbSet<Spice> Spices { get; set; }
  public DbSet<SpiceGroup> SpiceGroups { get; set; }

  public SpicesContext(DbContextOptions<SpicesContext> options) : base(options)
  {
    
  }
}