namespace job_application_tracker.Tests.Business.Creators
{
    using job_application_tracker.Business.Creators;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class UserCreatorTests : TestBase<UserCreator>
    {
        [Fact]
        public void CreateUser_HashesPasswordBeforeSaving()
        {
            // Arrange
            var request = new CreateUser
            {
                Email = "jordan@example.com", 
                Password = "Password123!"
            };
            
            var hashedPassword = "hashed_Password123!";

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
                .Returns(hashedPassword);

            User? capturedUser = null;

            this.automocker.GetMock<ICreateUserQuery>()
                .Setup(x => x.Execute(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns<User>(u => u);

            var sut = this.CreateTestSubject();

            // Act
            sut.CreateUser(request);

            // Assert
            Assert.NotNull(capturedUser);
            Assert.Equal(hashedPassword, capturedUser.PasswordHash);
        }

        [Fact]
        public void CreateUser_SetsEmailFromRequest()
        {
            // Arrange
            var request = new CreateUser
            {
                Email = "jordan@example.com", 
                Password = "Password123!"
            };

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
                .Returns("hashed");

            User? capturedUser = null;

            this.automocker.GetMock<ICreateUserQuery>()
                .Setup(x => x.Execute(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns<User>(u => u);

            var sut = this.CreateTestSubject();

            // Act
            sut.CreateUser(request);

            // Assert
            Assert.NotNull(capturedUser);
            Assert.Equal(request.Email, capturedUser.Email);
        }

        [Fact]
        public void CreateUser_ReturnsResultFromQuery()
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
                PasswordHash = "hashed"
            };

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
                .Returns("hashed");

            this.automocker.GetMock<ICreateUserQuery>()
                .Setup(x => x.Execute(It.IsAny<User>()))
                .Returns(expectedUser);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.CreateUser(request);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("jordan@example.com", result.Email);
        }
    }
}