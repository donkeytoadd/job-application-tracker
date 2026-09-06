namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;

    public interface IGetUserByEmailQuery
    {
        User? Execute(string email);
    }
}