using System.Reflection;
using AutoMapper;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzyprawTests;

public class SpiceGroupRepositoryTest : IClassFixture<DatabaseFixture>
{
  public DatabaseFixture Fixture { get; }

  public SpiceGroupRepositoryTest(DatabaseFixture fixture)
  {
    Fixture = fixture;
  }

  [Fact]
  public async void GetAllSpiceGroupsAsync_NotEmpty()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var spiceGroups = await spiceGroupRepository.GetAllSpiceGroupsAsync();
    Assert.NotEmpty(spiceGroups);
  }

  [Fact]
  public async void GetSpiceGroupByIdAsync_ReturnsCorrectSpiceGroup()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var firstSpiceGroup = context.SpiceGroups.First();
    var foundSpiceGroup = await spiceGroupRepository.GetSpiceGroupByIdAsync(firstSpiceGroup.SpiceGroupId);
    Assert.Equal(firstSpiceGroup.SpiceGroupId, foundSpiceGroup.SpiceGroupId);
  }

  [Fact]
  public async void GetSpiceGroupByNameAsync_ReturnsWithCorrectName()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var firstSpiceGroup = context.SpiceGroups.First();
    var foundSpiceGroup = await spiceGroupRepository.GetSpiceGroupByNameAsync(firstSpiceGroup.Name);
    Assert.Equal(firstSpiceGroup.Name, foundSpiceGroup.Name);
  }

  [Fact]
  public async void GetSpiceGroupByNameAsync_InExactDrawer_ReturnsWithCorrectName()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var firstSpiceGroup = context.SpiceGroups.First();
    var firstDrawer = context.Drawers.First();
    var foundSpiceGroup =
      await spiceGroupRepository.GetSpiceGroupByNameAsync(firstSpiceGroup.Name, firstDrawer.DrawerId);

    Assert.Equal(firstSpiceGroup.Name, foundSpiceGroup.Name);
    Assert.Equal(firstSpiceGroup.DrawerId, foundSpiceGroup.DrawerId);
  }

  [Fact]
  public async void GetSpiceGroupByBarcode_ReturnsWithCorrectBarcode()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var firstSpiceGroup = context.SpiceGroups.First();
    var foundSpiceGroup =
      await spiceGroupRepository.GetSpiceByBarcodeAsync(firstSpiceGroup.Barcode);

    Assert.NotNull(foundSpiceGroup);
    Assert.Equal(firstSpiceGroup.Barcode, foundSpiceGroup.Barcode);
  }

  [Fact]
  public async void AddSpiceGroupAsync_CorrectlyCreated()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var firstDrawer = context.Drawers.First();
    await unitOfWork.BeginTransaction();
    await spiceGroupRepository.AddSpiceGroupAsync(
      new SpiceGroup
      {
        Name = "Test1",
        Barcode = "2222222222222",
        Grams = 10,
        DrawerId = firstDrawer.DrawerId,
        MinimumGrams = 2
      });

    var foundSpiceGroup = context.SpiceGroups.First(group => group.Name == "Test1");
    Assert.NotNull(foundSpiceGroup);
    await unitOfWork.Rollback();
  }

  [Fact]
  public async void TransferSpiceGroupsAsync_CorrectlyTransferred()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    var twoDrawers = context.Drawers.ToList().Take(2).ToList();
    var firstDrawer = twoDrawers[0];
    var secondDrawer = twoDrawers[1];

    await unitOfWork.BeginTransaction();
    await spiceGroupRepository.TransferSpiceGroupsAsync(firstDrawer.DrawerId, secondDrawer.DrawerId);

    Assert.Empty(firstDrawer.SpiceGroups);
    await unitOfWork.Rollback();
  }

  [Fact]
  public async void UpdateSpiceGroupsAsync_CorrectlyUpdated()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstSpiceGroup = context.SpiceGroups.First();
    firstSpiceGroup.Name = "Test1";
    firstSpiceGroup.Barcode = "2222222222222";
    firstSpiceGroup.Grams = 54;
    firstSpiceGroup.MinimumCount = 2;

    var mapper = new Mapper(new MapperConfiguration(cfg =>
      cfg.AddMaps(Assembly.GetAssembly(typeof(AutoMapperProfile.OrganizationProfile)))));
    var copy = mapper.Map(firstSpiceGroup, new SpiceGroup());
    await spiceGroupRepository.UpdateSpiceGroupAsync(firstSpiceGroup);
    var found = context.SpiceGroups.Find(firstSpiceGroup.SpiceGroupId);
    Assert.Equal(copy, found);
    await unitOfWork.Rollback();
  }

  [Fact]
  public async void RemoveSpiceGroupsAsync_CorrectlyRemoved()
  {
    await using var context = Fixture.CreateContext();
    var unitOfWork = new UnitOfWork<SpicesContext>(context);
    var spiceGroupRepository = new SpiceGroupRepository(unitOfWork);

    await unitOfWork.BeginTransaction();
    var firstSpiceGroup = context.SpiceGroups.First();
    await spiceGroupRepository.RemoveSpiceGroupAsync(firstSpiceGroup.SpiceGroupId);
    var found = context.SpiceGroups.Find(firstSpiceGroup.SpiceGroupId);
    Assert.Null(found);
    await unitOfWork.Rollback();
  }
}