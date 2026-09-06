namespace job_application_tracker.Controllers
{
    using Business.Authenticators.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Resources;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthenticator authenticator;

        public AuthController(ILogger<AuthController> logger, IAuthenticator authenticator)
        {
            this.logger = logger;
            this.authenticator = authenticator;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            logger.LogInformation("Login attempt for {Email}", request.Email);
            var response = this.authenticator.Authenticate(request);

            if (response == null)
                return Unauthorized();

            return Ok(response);
        }
    }
}