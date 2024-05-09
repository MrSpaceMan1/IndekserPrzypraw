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
      CreateMap<Drawer, DrawerDTO>()
        .ForMember(dest => dest.Spices, opt => opt.MapFrom(src => src.SpiceGroups));
      CreateMap<Spice, SpiceDTO>()
        .ForMember(dest => dest.Grams, opt => opt.MapFrom(src => src.SpiceGroup.Grams))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SpiceGroup.Name))
        .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.SpiceGroup.Barcode));
      CreateMap<AddDrawerDTO, Drawer>();
      CreateMap<SpiceGroup, BarcodeInfoDTO>();
      CreateMap<SpiceGroup, SpiceGroupDTO>()
        .ForMember(dest => dest.SpiceGroupId, opt => opt.MapFrom(src => src.SpiceGroupId))
        .ForMember(dest => dest.MinimumGrams, opt => opt.AllowNull())
        .ForMember(dest => dest.MinimumCount, opt => opt.AllowNull());
      CreateMap<UpdateSpiceGroupDTO, SpiceGroup>()
        .ForAllMembers(opt => opt.Condition((dto, group, value) => value is not null));
    }
  }
}