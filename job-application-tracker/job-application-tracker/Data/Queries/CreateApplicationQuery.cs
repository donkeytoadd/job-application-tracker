namespace job_application_tracker.Data.Queries
{
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CreateApplicationQuery : ICreateApplicationQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public CreateApplicationQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public JobApplication Execute(JobApplication jobApplication)
        {
            try
            {
                using (var context = this.contextFactory.CreateDbContext())
                {
                    context.JobApplication.Add(jobApplication);
                    context.SaveChanges();

                    return jobApplication;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating job application: " + ex.Message, ex);
            }
        }
    }
}