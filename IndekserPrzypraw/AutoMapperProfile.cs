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
      CreateMap<Drawer, DrawerDTO>().ReverseMap();
      
      CreateMap<Spice, SpiceDTO>()
        .ForCtorParam("SpiceId", opt => opt.MapFrom(src => src.SpiceId))
        .ForCtorParam("Name", opt => opt.MapFrom(src => src.SpiceGroup.Name))
        .ForCtorParam("ExpirationDate", opt => opt.MapFrom(src => src.ExpirationDate))
        .ForCtorParam("Grams", opt => opt.MapFrom(src => src.SpiceGroup.Grams))
        .ForCtorParam("SpiceGroupId", opt => opt.MapFrom(src => src.SpiceGroupId))
        .ForCtorParam("DrawerId", opt => opt.MapFrom(src => src.DrawerId))
        .ForCtorParam("Barcode", opt => opt.MapFrom(src => src.SpiceGroup.Barcode));
      
      CreateMap<AddDrawerDTO, Drawer>().ReverseMap();
      
      CreateMap<SpiceGroup, BarcodeInfoDTO>();
      
      CreateMap<IEnumerable<Spice>, Dictionary<String, List<Spice>>>()
        .ConstructUsing(list =>
          list.GroupBy(spice => spice.SpiceGroup.Name,
            (s, spices) => new { Key = s, Spices = spices }).ToDictionary(
            pair => pair.Key,
            pair => pair.Spices.ToList()
          ));

      CreateMap<IEnumerable<Spice>, List<SpiceGroupDTO>>()
        .ConstructUsing((list, ctx) => list
          .GroupBy(spice => spice.SpiceGroup.Name)
          .Select(group => new SpiceGroupDTO{
          Name = group.Key,
          Spices = ctx.Mapper.Map<IEnumerable<Spice>, List<SpiceDTO>>(group.ToList())
          }).ToList());

      CreateMap<Spice, KeyValuePair<String, List<SpiceDTO>>>()
        .ConstructUsing((spice, cxt) =>
          new KeyValuePair<string, List<SpiceDTO>>(spice.SpiceGroup.Name, [cxt.Mapper.Map<SpiceDTO>(spice)]));
    }
  }
}