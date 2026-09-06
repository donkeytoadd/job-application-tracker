namespace job_application_tracker.Data.Queries
{
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CreateUserQuery :  ICreateUserQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public CreateUserQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public User Execute(User user)
        {
            try
            {
                using (var context = this.contextFactory.CreateDbContext())
                {
                    context.User.Add(user);
                    context.SaveChanges();
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user: " + ex.Message, ex);
            }
        }
    }
}