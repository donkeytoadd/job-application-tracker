namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;

    public interface IGetApplicationsByUserIdQuery
    {
        List<JobApplication> Execute(int userId);
    }
}