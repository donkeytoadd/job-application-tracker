namespace job_application_tracker.Data.Queries.Interfaces
{
    using Entities;

    public interface ICreateUserQuery
    {
        User Execute(User user);
    }
}