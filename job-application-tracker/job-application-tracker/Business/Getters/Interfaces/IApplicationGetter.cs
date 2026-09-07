namespace job_application_tracker.Business.Getters.Interfaces
{
    using Data.Entities;

    public interface IApplicationGetter
    {
        List<JobApplication> GetApplicationsByUserId(int userId);
        JobApplication? GetApplicationById(int id);
    }
}