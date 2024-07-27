namespace tutorialAPI.Models.Dto
{
    public class AddEmployeeDto
    {
        public required string depID { get; set; }
        public required string fullname { get; set; }
        public required string created_at { get; set; }
        public string? imagePath { get; set; }
    }
}
