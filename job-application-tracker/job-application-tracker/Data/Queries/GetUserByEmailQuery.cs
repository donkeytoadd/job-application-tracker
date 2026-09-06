namespace job_application_tracker.Data.Queries
{
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GetUserByEmailQuery : IGetUserByEmailQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public GetUserByEmailQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public User? Execute(string email)
        {
            using var context = this.contextFactory.CreateDbContext();
            return context.User.FirstOrDefault(u => u.Email == email);
        }
    }
}