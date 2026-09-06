namespace job_application_tracker.Tests.Controllers
{
    using job_application_tracker.Business.Creators.Interfaces;
    using job_application_tracker.Controllers;
    using job_application_tracker.Data.Entities;
    using Microsoft.AspNetCore.Mvc;

    public class UserControllerTests : TestBase<UserController>
    {
        [Fact]
        public void CreateUser_ReturnsOkWithCreatedUser()
        {
            // Arrange
            var request = new CreateUser
            {
                Email = "jordan@example.com",
                Password = "Password123!"
            };

            var expectedUser = new User
            {
                Id = 1,
                Email = request.Email,
                PasswordHash = "hashed_password"
            };

            this.automocker.GetMock<IUserCreator>()
                .Setup(x => x.CreateUser(request))
                .Returns(expectedUser);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.CreateUser(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal(1, user.Id);
            Assert.Equal("jordan@example.com", user.Email);
        }
    }
}