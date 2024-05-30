using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzyprawTests;

public class SpiceRepositoryTest : IClassFixture<DatabaseFixture>
{
  public DatabaseFixture Fixture { get; }

  public SpiceRepositoryTest(DatabaseFixture fixture)
  {
    Fixture = fixture;
  }

  [Fact]
  public async void GetAllSpices_NotEmpty()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceRepository = new SpiceRepository(unitOfWork);
    var spices = await spiceRepository.GetAllSpicesAsync();
    Assert.NotEmpty(spices.ToList());
  }

  [Fact]
  public async void GetSpiceById_NotNull()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceRepository = new SpiceRepository(unitOfWork);
    var spice = await spiceRepository.GetSpiceByIdAsync(1);
    Assert.NotNull(spice);
  }

  [Fact]
  public async void GetAllSpicesFromDrawer_AllSpicesInCorrectDrawer()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceRepository = new SpiceRepository(unitOfWork);
    var testDrawer = context.Drawers.First();
    var spices = await spiceRepository.GetAllSpicesFromDrawerAsync(testDrawer.DrawerId);

    Assert.All(spices, spice => Assert.Equal(testDrawer.DrawerId, spice.SpiceGroup.DrawerId));
  }

  [Fact]
  public async void AddSpiceAsync_AddsSpice()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceRepository = new SpiceRepository(unitOfWork);
    await unitOfWork.BeginTransaction();
    var spiceGroup = context.SpiceGroups.First();
    var spiceToAdd = new Spice
      { SpiceGroupId = spiceGroup.SpiceGroupId, ExpirationDate = new(2024, 5, 30) };
    await spiceRepository.AddSpiceAsync(spiceToAdd);
    await unitOfWork.Save();
    var found = context.Spices.Find(spiceToAdd.SpiceId);
    Assert.Equal(spiceToAdd, found);
    await unitOfWork.Rollback();
  }

  [Fact]
  public async void DeleteSpiceAsync_CorrectlyRemovesSpice()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceRepository = new SpiceRepository(unitOfWork);
    await unitOfWork.BeginTransaction();
    var spiceToDelete = context.Spices.First();
    await spiceRepository.DeleteSpiceAsync(spiceToDelete);
    await unitOfWork.Save();
    var found = context.Spices.Find(spiceToDelete.SpiceId);
    Assert.Null(found);
    await unitOfWork.Rollback();
  }
}