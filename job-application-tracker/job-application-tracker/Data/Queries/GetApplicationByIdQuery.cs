namespace job_application_tracker.Data.Queries
{
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GetApplicationByIdQuery : IGetApplicationByIdQuery
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public GetApplicationByIdQuery(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public JobApplication? Execute(int id)
        {
            using (var context = this.contextFactory.CreateDbContext())
            {
                return context.JobApplication
                    .Include(x => x.StatusHistory)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
