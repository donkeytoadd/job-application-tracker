namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;

    public interface ICreateApplicationQuery
    {
        JobApplication Execute(JobApplication jobApplication);
    }
}