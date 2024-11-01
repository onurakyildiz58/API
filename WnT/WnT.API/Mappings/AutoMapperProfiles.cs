using AutoMapper;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO.difficulty;
using WnT.API.Models.DTO.image;
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

            CreateMap<AddImageDTO, Image>()
                .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.File.Length))
                .ForMember(dest => dest.FilePath, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
