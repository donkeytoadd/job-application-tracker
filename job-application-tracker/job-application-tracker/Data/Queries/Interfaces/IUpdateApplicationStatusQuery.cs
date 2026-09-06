namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;
    using Enums;

    public interface IUpdateApplicationStatusQuery
    {
        JobApplication? Execute(int applicationId, ApplicationStatus newStatus);
    }
}