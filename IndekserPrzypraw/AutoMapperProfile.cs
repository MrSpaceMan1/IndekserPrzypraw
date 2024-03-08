using AutoMapper;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Models;

namespace IndekserPrzypraw.Profiles;

public class AutoMapperProfile
{
  public class OrganizationProfile : Profile
  {
    public OrganizationProfile()
    {
      CreateMap<Spice, SpiceDTO>()
        .ForCtorParam("SpiceId", opt => opt.MapFrom(src => src.SpiceId))
        .ForCtorParam("Name", opt => opt.MapFrom(src => src.SpiceGroup.Name))
        .ForCtorParam("ExpirationDate", opt => opt.MapFrom(src => src.ExpirationDate))
        .ForCtorParam("Grams", opt => opt.MapFrom(src => src.Grams))
        .ForCtorParam("SpiceGroupId", opt => opt.MapFrom(src => src.SpiceGroupId))
        .ForCtorParam("DrawerId", opt => opt.MapFrom(src => src.DrawerId));
      // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)  
      CreateMap<Drawer, DrawerDTO>();
      CreateMap<AddDrawerDTO, Drawer>().ReverseMap();
    }
  }
}