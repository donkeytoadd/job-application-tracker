namespace job_application_tracker.Business.Creators.Interfaces
{
    using Data.Entities;

    public interface IUserCreator
    {
        User CreateUser(CreateUser request);
    }
}