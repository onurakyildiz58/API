using System.ComponentModel.DataAnnotations.Schema;

namespace WnT.API.Models.DTO.image
{
    public class AddImageDTO
    {
        [NotMapped]
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
