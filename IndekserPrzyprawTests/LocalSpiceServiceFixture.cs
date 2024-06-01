using System.Reflection;
using AutoMapper;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;
using Moq;

namespace IndekserPrzyprawTests;

public class LocalSpiceServiceFixture
{
  public Mock<ISpiceRepository> GetSpiceRepositoryMock =>
    new Mock<ISpiceRepository>();

  public Mock<ISpiceGroupRepository> GetSpiceGroupRepositoryMock =>
    new Mock<ISpiceGroupRepository>();

  public Mock<IDrawerRepository> GetDrawerRepositoryMock =>
    new Mock<IDrawerRepository>();

  public IMapper GetMapper => new Mapper(new MapperConfiguration(cfg =>
    cfg.AddMaps(Assembly.GetAssembly(typeof(AutoMapperProfile.OrganizationProfile)))));
}