namespace job_application_tracker.Data.Entities
{
    public class CreateUser
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}