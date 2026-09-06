namespace job_application_tracker.Controllers
{
    using Business.Creators.Interfaces;
    using Business.Getters.Interfaces;
    using Data.Entities;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationController : ControllerBase
    {
        private readonly ILogger<JobApplicationController> logger;
        private readonly IApplicationGetter applicationGetter;
        private readonly IApplicationCreator applicationCreator;
        
        public JobApplicationController(
            ILogger<JobApplicationController> logger,
            IApplicationGetter applicationGetter,
            IApplicationCreator applicationCreator)
        {
            this.logger = logger;
            this.applicationGetter = applicationGetter;
            this.applicationCreator = applicationCreator;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<JobApplication>> GetJobApplicationsByUserId(int userId)
        {
            logger.LogInformation("Getting all applications for UserId {UserId}", userId);
            var applicationList = this.applicationGetter.GetApplicationsByUserId(userId);
            return Ok(applicationList);
        }

        [HttpPost]
        public ActionResult<JobApplication> CreateJobApplication(CreateApplication createApplicationRequest)
        {
            logger.LogInformation("Creating new JobApplication for UserId {UserId}", createApplicationRequest.UserId);
            var created = this.applicationCreator.CreateJobApplication(createApplicationRequest);
            return Ok(created);
        }
    }
}