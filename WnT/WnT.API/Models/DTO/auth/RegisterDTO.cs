using System.ComponentModel.DataAnnotations;

namespace WnT.API.Models.DTO.auth
{
    public class RegisterDTO
    {
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public required string[] roles { get; set; }
    }
}
