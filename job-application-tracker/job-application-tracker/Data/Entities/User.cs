namespace job_application_tracker.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public List<JobApplication> Applications { get; set; } = new();
    }
}