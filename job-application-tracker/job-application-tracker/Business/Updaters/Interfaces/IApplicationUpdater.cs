namespace job_application_tracker.Business.Updaters.Interfaces
{
    using Data.Entities;
    using Data.Enums;

    public interface IApplicationUpdater
    {
        JobApplication? UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus);
    }
}