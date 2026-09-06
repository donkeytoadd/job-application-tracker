namespace job_application_tracker.Data.Entities
{
    using Enums;

    public class JobApplication
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Company { get; set; }
        public required string Role { get; set; }
        public string? JobUrl { get; set; }
        public string? Notes { get; set; }

        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
        
        public List<ApplicationStatusHistory> StatusHistory { get; set; } = new();
    }
}