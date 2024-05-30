using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IndekserPrzyprawTests;

public class DrawerRepositoryTest : IClassFixture<DatabaseFixture>
{
  public DatabaseFixture Fixture { get; }

  public DrawerRepositoryTest(DatabaseFixture fixture)
  {
    Fixture = fixture;
  }

  [Fact]
  public async void GetDrawerByIdAsync_NotNull()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    var firstDrawer = context.Drawers.First();
    var foundDrawer = await drawerRepository.GetDrawerByIdAsync(firstDrawer.DrawerId);

    Assert.Equal(firstDrawer.DrawerId, foundDrawer.DrawerId);
  }

  [Fact]
  public async void GetAllDrawerAsync_NotNull()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    var drawers = await drawerRepository.GetAllDrawersAsync();
    Assert.NotEmpty(drawers);
  }

  [Fact]
  public async void AddDrawerAsync_CorrectlyAdded()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var drawerToAdd = new Drawer { Name = "Test1" };
    await drawerRepository.AddDrawerAsync(drawerToAdd);
    var found = context.Drawers.First(drawer => drawer.Name == "Test1");

    Assert.Equal(drawerToAdd, found);

    await unitOfWork.Rollback();
  }

  [Fact]
  public async void RemoveDrawerAsync_CorrectlyRemoved()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstDrawer = context.Drawers.Include(drawer => drawer.SpiceGroups).First();
    await drawerRepository.RemoveDrawerAsync(firstDrawer);
    var foundDrawer = context.Drawers.Find(firstDrawer.DrawerId);
    Assert.Null(foundDrawer);

    await unitOfWork.Rollback();
  }


  [Fact]
  public async void UpdateDrawerAsync_Updated()
  {
  }
}