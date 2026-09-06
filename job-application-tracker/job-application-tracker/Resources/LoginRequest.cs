namespace job_application_tracker.Resources
{
    public class LoginRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}