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
  public async void RemoveDrawerAsync_DrawerWithSpicesThrowsInvalidOperationException()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstDrawer = context.Drawers.Include(drawer => drawer.SpiceGroups).First();
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await drawerRepository.RemoveDrawerAsync(firstDrawer));

    await unitOfWork.Rollback();
  }

  [Fact]
  public async void RemoveDrawerAsync_DrawerWithNoSpicesSuccessfulyRemoves()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstDrawer = context.Drawers.Include(drawer => drawer.SpiceGroups).First();
    context.RemoveRange(firstDrawer.SpiceGroups);
    await drawerRepository.RemoveDrawerAsync(firstDrawer);
    var found = context.Drawers.FirstOrDefault(drawer => drawer.DrawerId == firstDrawer.DrawerId);
    Assert.Null(found);
    
    await unitOfWork.Rollback();
  }

  [Fact]
  public async void UpdateDrawerAsync_Updated() 
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var drawerRepository = new DrawerRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstDrawer = context.Drawers.First();
    firstDrawer.Name = "Test1";
    await drawerRepository.UpdateDrawerAsync(firstDrawer);
    var foundDrawer = context.Drawers.Find(firstDrawer.DrawerId);
    Assert.Equal(firstDrawer, foundDrawer);
    
    await unitOfWork.Rollback();
  }
}