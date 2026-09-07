namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;

    public interface IGetApplicationByIdQuery
    {
        JobApplication? Execute(int id);
    }
}
