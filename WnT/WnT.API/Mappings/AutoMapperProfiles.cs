using AutoMapper;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO.difficulty;
using WnT.API.Models.DTO.region;
using WnT.API.Models.DTO.walk;

namespace WnT.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();

            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<AddWalkDTO, Walk>().ReverseMap();
            CreateMap<UpdateWalkDTO, Walk>().ReverseMap();
            
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
        }
    }
}
