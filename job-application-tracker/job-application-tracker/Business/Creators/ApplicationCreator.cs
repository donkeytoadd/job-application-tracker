namespace job_application_tracker.Business.Creators
{
    using Data.Entities;
    using Data.Queries.Interfaces;
    using Interfaces;

    public class ApplicationCreator : IApplicationCreator
    {
        private readonly ICreateApplicationQuery createApplicationQuery;
        
        public ApplicationCreator(ICreateApplicationQuery createApplicationQuery)
        {
            this.createApplicationQuery = createApplicationQuery;
        }
        
        public JobApplication CreateJobApplication(CreateApplication createApplicationRequest)
        {
            var mappedApplication = new JobApplication
            {
                UserId = createApplicationRequest.UserId,
                Company = createApplicationRequest.Company,
                Role = createApplicationRequest.Role,
                JobUrl = createApplicationRequest.JobUrl,
                Notes = createApplicationRequest.Notes,
                StatusHistory = new List<Data.Entities.ApplicationStatusHistory>
                {
                    new() { Status = Data.Enums.ApplicationStatus.Applied }
                }
            };

            var createdApplication = this.createApplicationQuery.Execute(mappedApplication);
            
            return createdApplication;
        }
    }
}