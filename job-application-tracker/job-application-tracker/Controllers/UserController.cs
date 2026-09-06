namespace job_application_tracker.Controllers
{
    using Business.Creators.Interfaces;
    using Data.Entities;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserCreator userCreator;

        public UserController(
            ILogger<UserController> logger,
            IUserCreator userCreator)
        {
            this.logger = logger;
            this.userCreator = userCreator;
        }

        [HttpPost]
        public ActionResult<User> CreateUser(CreateUser createUserRequest)
        {
            logger.LogInformation("Creating new user with email {Email}", createUserRequest.Email);
            var user = this.userCreator.CreateUser(createUserRequest);
            return Ok(user);
        }
    }
}