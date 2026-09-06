namespace job_application_tracker.Business.Creators
{
    using Data.Entities;
    using Data.Queries.Interfaces;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;

    public class UserCreator : IUserCreator
    {
        private readonly ICreateUserQuery createUserQuery;
        private readonly IPasswordHasher<User> passwordHasher;

        public UserCreator(ICreateUserQuery createUserQuery, IPasswordHasher<User> passwordHasher)
        {
            this.createUserQuery = createUserQuery;
            this.passwordHasher = passwordHasher;
        }

        public User CreateUser(CreateUser request)
        {
            var user = new User
            {
                Email = request.Email,
                PasswordHash = this.passwordHasher.HashPassword(null!, request.Password)
            };

            return this.createUserQuery.Execute(user);
        }
    }
}