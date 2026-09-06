namespace job_application_tracker.Data.Entities
{
    public class CreateApplication
    {
        public int UserId { get; set; }
        public required string Company { get; set; }
        public required string Role { get; set; }
        public string? JobUrl { get; set; }
        public string? Notes { get; set; }
    }
}