namespace job_application_tracker.Data.Entities
{
    using System.Text.Json.Serialization;
    using Enums;

    public class ApplicationStatusHistory
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }
        [JsonIgnore]
        public JobApplication? Application { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}