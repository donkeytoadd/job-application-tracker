namespace job_application_tracker.Resources
{
    public class LoginResponse
    {
        public int UserId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }
    }
}