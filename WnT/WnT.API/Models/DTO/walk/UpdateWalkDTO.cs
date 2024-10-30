using System.ComponentModel.DataAnnotations;

namespace WnT.API.Models.DTO.walk
{
    public class UpdateWalkDTO
    {

        [MaxLength(100, ErrorMessage = "Yol ismi maksimum 100 karakterden oluşmalıdır")]
        public required string Name { get; set; }

        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olmalıdır")]
        public required string Description { get; set; }

        [Range(0, 50, ErrorMessage = "Uzunluğu 0-50 km arası olmalıdır")]
        public required double LengthInKM { get; set; }

        public string? WalkImageUrl { get; set; }

        public required Guid DifficultyId { get; set; }
        public required Guid RegionId { get; set; }
    }
}
