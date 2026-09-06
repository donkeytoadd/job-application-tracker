namespace job_application_tracker.Business.Creators.Interfaces
{
    using Data.Entities;

    public interface IApplicationCreator
    {
        JobApplication CreateJobApplication(CreateApplication createApplicationRequest);
    }
}