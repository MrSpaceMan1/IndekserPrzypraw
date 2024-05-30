using IndekserPrzypraw.Models;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzyprawTests;

using IndekserPrzypraw.Data;

public class DatabaseFixture
{
  private const string _connectionString =
    "Host=localhost;Port=5432;Username=postgres;Password=password;Database=test";

  private static readonly object _lock = new();
  private static bool _databaseInitialized;

  public DatabaseFixture()
  {
    lock (_lock)
    {
      if (!_databaseInitialized)
      {
        using (var context = CreateContext())
        {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();

          var drawer1 = new Drawer { Name = "Drawer1" };
          var drawer2 = new Drawer { Name = "Drawer2" };

          context.AddRange(
            drawer1,
            drawer2);

          var spiceGroups = new List<SpiceGroup>
          {
            new SpiceGroup
              { Name = "SpiceGroup1", Drawer = drawer1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3 },
            new SpiceGroup
              { Name = "SpiceGroup2", Drawer = drawer1, Grams = 7, Barcode = "1111111111112", MinimumGrams = 5 },
            new SpiceGroup
              { Name = "SpiceGroup3", Drawer = drawer1, Grams = 2, Barcode = "1111111111113", MinimumGrams = 0 },
            new SpiceGroup
              { Name = "SpiceGroup4", Drawer = drawer1, Grams = 5, Barcode = "1111111111114", MinimumGrams = 1 },
            new SpiceGroup
              { Name = "SpiceGroup5", Drawer = drawer2, Grams = 5, Barcode = "1111111111115", MinimumGrams = 5 },
            new SpiceGroup
              { Name = "SpiceGroup6", Drawer = drawer2, Grams = 1, Barcode = "1111111111116", MinimumGrams = 2 },
            new SpiceGroup
              { Name = "SpiceGroup7", Drawer = drawer2, Grams = 3, Barcode = "1111111111117", MinimumGrams = 8 },
            new SpiceGroup
              { Name = "SpiceGroup8", Drawer = drawer2, Grams = 15, Barcode = "1111111111118", MinimumGrams = 10 }
          };

          context.AddRange(
          );

          foreach (var spiceGroup in spiceGroups)
          {
            context.AddRange(
              new Spice { SpiceGroup = spiceGroup },
              new Spice { SpiceGroup = spiceGroup }
            );
          }

          context.SaveChanges();
        }

        _databaseInitialized = true;
      }
    }
  }

  public SpicesContext CreateContext()
    => new SpicesContext(
      new DbContextOptionsBuilder<SpicesContext>()
        .UseNpgsql(_connectionString)
        .Options);
}