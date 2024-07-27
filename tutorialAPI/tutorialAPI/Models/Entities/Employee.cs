namespace tutorialAPI.Models.Entities
{
    public class Employee
    {
        public Guid id { get; set; }     
        public required string depID { get; set; }
        public required string fullname { get; set; }
        public required string created_at { get; set; }
        public string? imagePath { get; set; }
    }
}
