namespace job_application_tracker.Business.Authenticators.Interfaces
{
    using Resources;

    public interface IAuthenticator
    {
        LoginResponse? Authenticate(LoginRequest request);
    }
}