namespace job_application_tracker.Business.Authenticators
{
    using Data.Entities;
    using Data.Queries.Interfaces;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Resources;

    public class Authenticator : IAuthenticator
    {
        private readonly IGetUserByEmailQuery getUserByEmailQuery;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly ILogger<Authenticator> logger;

        public Authenticator(IGetUserByEmailQuery getUserByEmailQuery, IPasswordHasher<User> passwordHasher, ILogger<Authenticator> logger)
        {
            this.getUserByEmailQuery = getUserByEmailQuery;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        }

        public LoginResponse? Authenticate(LoginRequest request)
        {
            var user = this.getUserByEmailQuery.Execute(request.Email);

            if (user == null)
            {
                logger.LogWarning("Login failed: no user found for email {Email}", request.Email);
                return null;
            }

            var result = this.passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                logger.LogWarning("Login failed: incorrect password for email {Email}", request.Email);
                return null;
            }

            logger.LogInformation("Login successful for {Email}", request.Email);
            return new LoginResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}