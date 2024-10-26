using AutoMapper;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO;

namespace WnT.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();

        }
    }
}
