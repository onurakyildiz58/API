using System.ComponentModel.DataAnnotations;

namespace WnT.API.Models.DTO.region
{
    public class AddRegionDTO
    {
        [MinLength(3, ErrorMessage = "kod 3 karakterli olmalıdır")]
        [MaxLength(3, ErrorMessage = "kod 3 karakterli olmalıdır")]
        public required string Code { get; set; }
        [MaxLength(100, ErrorMessage = "Bölge ismi en fazla 100 karakterli olmalıdır")]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
