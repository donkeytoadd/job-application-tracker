namespace job_application_tracker.Business.Getters
{
    using Data.Entities;
    using Data.Queries.Interfaces;
    using Interfaces;

    public class ApplicationGetter : IApplicationGetter
    {
        private readonly IGetApplicationsByUserIdQuery getApplicationsByUserIdQuery;
        private readonly IGetApplicationByIdQuery getApplicationByIdQuery;

        public ApplicationGetter(
            IGetApplicationsByUserIdQuery getApplicationsByUserIdQuery,
            IGetApplicationByIdQuery getApplicationByIdQuery)
        {
            this.getApplicationsByUserIdQuery = getApplicationsByUserIdQuery;
            this.getApplicationByIdQuery = getApplicationByIdQuery;
        }

        public List<JobApplication> GetApplicationsByUserId(int userId)
        {
            var applicationList = this.getApplicationsByUserIdQuery.Execute(userId);
            return applicationList.Count > 0 ? applicationList : new List<JobApplication>();
        }

        public JobApplication? GetApplicationById(int id)
        {
            return this.getApplicationByIdQuery.Execute(id);
        }
    }
}