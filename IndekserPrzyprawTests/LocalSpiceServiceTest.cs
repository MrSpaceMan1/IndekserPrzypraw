using System.Collections.ObjectModel;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Services;
using IndekserPrzyprawTests.Mocks;
using Moq;


namespace IndekserPrzyprawTests;

public class LocalSpiceServiceTest : IClassFixture<LocalSpiceServiceFixture>
{
  private LocalSpiceServiceFixture _fixture;

  public LocalSpiceServiceTest(LocalSpiceServiceFixture fixture)
  {
    _fixture = fixture;
  }

  [Fact]
  public async void GetAllSpicesAsync_ReturnsAllSpice()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var drawer1 = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer1, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spiceGroup2 = new SpiceGroup
    {
      Name = "SpiceGroup2", Drawer = drawer1, DrawerId = 1, Grams = 7, Barcode = "1111111111112", MinimumGrams = 5,
      SpiceGroupId = 2
    };
    var spices = new List<Spice>
    {
      new() { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup },
      new() { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 2, SpiceGroup = spiceGroup2 }
    };
    mockSpiceRepository.Setup(e => e.GetAllSpicesAsync()).ReturnsAsync(() => spices);

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);


    var foundSpices = await localSpiceService.GetAllSpicesAsync();
    Assert.Collection(foundSpices, spices.Select<Spice, Action<SpiceDTO>>(spice => (SpiceDTO dto) =>
    {
      Assert.Equal(spice.SpiceId, dto.SpiceId);
      Assert.Equal(spice.SpiceGroupId, dto.SpiceGroupId);
      Assert.Equal(spice.ExpirationDate, dto.ExpirationDate);
      Assert.Equal(spice.SpiceGroup.Barcode, dto.Barcode);
      Assert.Equal(spice.SpiceGroup.Name, dto.Name);
      Assert.Equal(spice.SpiceGroup.Grams, dto.Grams);
    }).ToArray());
  }

  [Fact]
  public async void GetSpiceByBarcodeAsync_CorrectSpiceFetched()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var drawer1 = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup2 = new SpiceGroup
    {
      Name = "SpiceGroup2", Drawer = drawer1, DrawerId = 1, Grams = 7, Barcode = "1111111111112", MinimumGrams = 5,
      SpiceGroupId = 2
    };
    var spice2 = new Spice { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 2, SpiceGroup = spiceGroup2 };
    mockSpiceGroupRepository.Setup(e => e.GetSpiceByBarcodeAsync("1111111111112"))
      .ReturnsAsync(() => spiceGroup2);

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var foundSpice = await localSpiceService.GetSpiceByBarcodeAsync("1111111111112");

    Assert.NotNull(foundSpice);
    Assert.Equal(spice2.SpiceGroup.Barcode, foundSpice.Barcode);
    Assert.Equal(spice2.SpiceGroup.Name, foundSpice.Name);
    Assert.Equal(spice2.SpiceGroup.Grams, foundSpice.Grams);
  }

  [Fact]
  public async void GetSpiceAsync_GetCorrectSpice()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var drawer1 = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup2 = new SpiceGroup
    {
      Name = "SpiceGroup2", Drawer = drawer1, DrawerId = 1, Grams = 7, Barcode = "1111111111112", MinimumGrams = 5,
      SpiceGroupId = 2
    };
    var spice2 = new Spice
      { SpiceId = 2, ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 2, SpiceGroup = spiceGroup2 };
    mockSpiceRepository.Setup(e => e.GetSpiceByIdAsync(2)).ReturnsAsync(() => spice2);

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var foundSpice = await localSpiceService.GetSpiceAsync(2);

    Assert.NotNull(foundSpice);
    Assert.Equal(spice2.SpiceGroupId, foundSpice.SpiceGroupId);
    Assert.Equal(spice2.SpiceId, foundSpice.SpiceId);
    Assert.Equal(spice2.SpiceGroup.Barcode, foundSpice.Barcode);
    Assert.Equal(spice2.ExpirationDate, foundSpice.ExpirationDate);
    Assert.Equal(spice2.SpiceGroup.Grams, foundSpice.Grams);
    Assert.Equal(spice2.SpiceGroup.Name, foundSpice.Name);
  }

  [Fact]
  public async void AddSpiceAsync_NoDrawerWithProvidedIdThrows()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    await Assert.ThrowsAsync<NotFoundException>(() => localSpiceService.AddSpiceAsync(1, new AddSpiceDTO(
      "Test1",
      5,
      new DateOnly(2024, 5, 31),
      "1111111111111"
    )));
  }

  [Fact]
  public async void AddSpiceAsync_AddsSpice()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { DrawerId = 1, Name = "Drawer1" };
    mockDrawer.Setup(e => e.GetDrawerByIdAsync(1)).ReturnsAsync(() => drawer);

    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByNameAsync("Test1", 1)).ReturnsAsync(() => null);
    mockSpiceGroupRepository.Setup(e => e.AddSpiceGroupAsync(It.IsAny<SpiceGroup>()))
      .ReturnsAsync((SpiceGroup sg) => new SpiceGroup
      {
        Barcode = sg.Barcode,
        Drawer = sg.Drawer,
        DrawerId = sg.DrawerId,
        Grams = sg.Grams,
        MinimumCount = sg.MinimumCount,
        MinimumGrams = sg.MinimumGrams,
        Name = sg.Name,
        SpiceGroupId = 1
      });
    mockSpiceRepository.Setup(e => e.AddSpiceAsync(It.IsAny<Spice>())).ReturnsAsync((Spice spice) => new Spice
    {
      ExpirationDate = spice.ExpirationDate,
      SpiceGroupId = spice.SpiceGroupId,
      SpiceGroup = spice.SpiceGroup,
      SpiceId = 1
    });

    var addSpiceDto = new AddSpiceDTO(
      "Test1",
      5,
      new DateOnly(2024, 5, 31),
      "1111111111111"
    );
    var dto = await localSpiceService.AddSpiceAsync(1, addSpiceDto);

    Assert.Equal(addSpiceDto.Name, dto.Name);
    Assert.Equal(addSpiceDto.ExpirationDate, dto.ExpirationDate);
    Assert.Equal(addSpiceDto.Barcode, dto.Barcode);
    Assert.Equal(addSpiceDto.Grams, dto.Grams);
  }

  [Fact]
  public async void GetSpicesInDrawerAsync_AllSpicesFromCorrectDrawer()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer1 = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer1, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spices = new List<Spice>
    {
      new() { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup },
      new() { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup }
    };

    mockSpiceRepository.Setup(e => e.GetAllSpicesFromDrawerAsync(1)).ReturnsAsync(spices);
    var dtos = await localSpiceService.GetSpicesInDrawerAsync(1);

    Assert.Collection(dtos, spices.Select<Spice, Action<SpiceDTO>>(spice => (SpiceDTO dto) =>
    {
      Assert.NotNull(dto);
      Assert.Equal(spice.SpiceGroupId, dto.SpiceGroupId);
      Assert.Equal(spice.SpiceId, dto.SpiceId);
      Assert.Equal(spice.SpiceGroup.Barcode, dto.Barcode);
      Assert.Equal(spice.ExpirationDate, dto.ExpirationDate);
      Assert.Equal(spice.SpiceGroup.Grams, dto.Grams);
      Assert.Equal(spice.SpiceGroup.Name, dto.Name);
    }).ToArray());
  }

  [Fact]
  public async void RemoveSpiceAsync_NoSpiceExists()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);


    mockSpiceRepository.Setup(e => e.GetSpiceByIdAsync(1)).ReturnsAsync(() => null);
    await Assert.ThrowsAsync<NotFoundException>(() => localSpiceService.RemoveSpiceAsync(1));
  }

  [Fact]
  public async void RemoveSpiceAsync_NoException()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spice = new Spice { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup };

    mockSpiceRepository.Setup(e => e.GetSpiceByIdAsync(1)).ReturnsAsync(() => spice);
    mockSpiceRepository.Setup(e => e.DeleteSpiceAsync(spice)).Returns(() => Task.CompletedTask);
    await localSpiceService.RemoveSpiceAsync(1);
  }

  [Fact]
  public async void RemoveSpiceGroupWithSpices_NoSpiceGroup()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spice = new Spice { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup };

    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByIdAsync(1)).ReturnsAsync(() => null);
    await Assert.ThrowsAsync<NotFoundException>(() => localSpiceService.RemoveSpiceGroupWithSpices(1));
  }

  [Fact]
  public async void RemoveSpiceGroupWithSpices()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spice1 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 1 };
    var spice2 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 2 };
    spiceGroup.Spices = new List<Spice> { spice1, spice2 };
    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByIdAsync(1)).ReturnsAsync(() => spiceGroup);
    mockSpiceRepository.Setup(e => e.DeleteSpiceAsync(It.IsAny<Spice>())).Returns(() => Task.CompletedTask);
    mockSpiceRepository.Setup(e => e.GetSpiceByIdAsync(It.IsAny<int>()))
      .ReturnsAsync((int id) => id == 1 ? spice1 : spice2);

    mockSpiceGroupRepository.Setup(e => e.RemoveSpiceGroupAsync(1)).Returns(() => Task.CompletedTask);
    await localSpiceService.RemoveSpiceGroupWithSpices(1);
  }

  [Fact]
  public async void UpdateSpiceGroup_NoSpiceGroup()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spice1 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 1 };
    var spice2 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 2 };
    spiceGroup.Spices = new List<Spice> { spice1, spice2 };
    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByIdAsync(1)).ReturnsAsync(() => null);
    await Assert.ThrowsAsync<NotFoundException>(() => localSpiceService.RemoveSpiceGroupWithSpices(1));
  }

  [Fact]
  public async void UpdateSpiceGroup()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 3,
      SpiceGroupId = 1
    };
    var spice1 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 1 };
    var spice2 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 2 };
    var spices = new List<Spice> { spice1, spice2 };
    spiceGroup.Spices = spices;
    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByIdAsync(1)).ReturnsAsync(() => spiceGroup);
    mockSpiceGroupRepository.Setup(e => e.UpdateSpiceGroupAsync(It.IsAny<SpiceGroup>()))
      .ReturnsAsync((SpiceGroup sg) => sg);
    var dto = await localSpiceService.UpdateSpiceGroup(1, new UpdateSpiceGroupDTO
    {
      MinimumGrams = 10,
      MinimumCount = 15,
      Name = "Test1"
    });

    Assert.Equal("Test1", dto.Name);
    Assert.Equal(10, dto.MinimumGrams);
    Assert.Equal(15, dto.MinimumCount);
    Assert.Equal(1, dto.SpiceGroupId);
  }

  [Fact]
  public async void GetMissingSpices()
  {
    var mockSpiceRepository = _fixture.GetSpiceRepositoryMock;
    var mockSpiceGroupRepository = _fixture.GetSpiceGroupRepositoryMock;
    var mockDrawer = _fixture.GetDrawerRepositoryMock;
    var mapper = _fixture.GetMapper;

    var localSpiceService = new LocalSpiceService(
      new UnitOfWorkStub<SpicesContext>(),
      mockSpiceGroupRepository.Object,
      mockSpiceRepository.Object,
      mockDrawer.Object,
      mapper);

    var drawer = new Drawer { Name = "Drawer1", DrawerId = 1 };
    var spiceGroup = new SpiceGroup
    {
      Name = "SpiceGroup1", Drawer = drawer, DrawerId = 1, Grams = 10, Barcode = "1111111111111", MinimumGrams = 25,
      SpiceGroupId = 1
    };
    drawer.SpiceGroups = [spiceGroup];
    var spice1 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 1 };
    var spice2 = new Spice
      { ExpirationDate = new DateOnly(2024, 5, 31), SpiceGroupId = 1, SpiceGroup = spiceGroup, SpiceId = 2 };
    var spices = new List<Spice> { spice1, spice2 };
    spiceGroup.Spices = spices;
    mockDrawer.Setup(e => e.GetAllDrawersAsync()).ReturnsAsync(() => [drawer]);
    mockSpiceGroupRepository.Setup(e => e.GetSpiceGroupByIdAsync(1)).ReturnsAsync(() => spiceGroup);
    mockSpiceGroupRepository.Setup(e => e.UpdateSpiceGroupAsync(It.IsAny<SpiceGroup>()))
      .ReturnsAsync((SpiceGroup sg) => sg);
    var missingSpices = await localSpiceService.GetMissingSpices();
    Assert.NotNull(missingSpices.MissingSpices["Drawer1"]);
    var missingSpiceGroup1 = missingSpices.MissingSpices["Drawer1"];
    ;
    Assert.Collection(missingSpiceGroup1,
      drawer.SpiceGroups.Select<SpiceGroup, Action<MissingSpiceGroupDTO>>(sg => (MissingSpiceGroupDTO dto) =>
      {
        var grams = sg.Spices.Count * sg.Grams;

        Assert.Equal(spiceGroup.MinimumGrams - grams, dto.MissingGrams);
        Assert.Equal(0, dto.MissingCount);
        Assert.Equal(sg.Name, dto.Name);
        Assert.Equal(sg.SpiceGroupId, dto.SpiceGroupId);
      }).ToArray());
  }
}