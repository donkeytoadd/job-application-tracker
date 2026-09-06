namespace job_application_tracker.Tests.Controllers
{
    using job_application_tracker.Business.Authenticators.Interfaces;
    using job_application_tracker.Controllers;
    using job_application_tracker.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class AuthControllerTests : TestBase<AuthController>
    {
        [Fact]
        public void Login_ReturnsOkWithLoginResponseOnValidCredentials()
        {
            // Arrange
            var request = new LoginRequest { Email = "jordan@example.com", Password = "Password123!" };

            var expectedResponse = new LoginResponse
            {
                UserId = 1,
                Name = "Jordan Hanson",
                Email = "jordan@example.com"
            };

            this.automocker.GetMock<IAuthenticator>()
                .Setup(x => x.Authenticate(request))
                .Returns(expectedResponse);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(1, response.UserId);
            Assert.Equal("Jordan Hanson", response.Name);
            Assert.Equal("jordan@example.com", response.Email);
        }

        [Fact]
        public void Login_ReturnsUnauthorizedWhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequest { Email = "jordan@example.com", Password = "WrongPassword!" };

            this.automocker.GetMock<IAuthenticator>()
                .Setup(x => x.Authenticate(request))
                .Returns((LoginResponse?)null);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Login(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}