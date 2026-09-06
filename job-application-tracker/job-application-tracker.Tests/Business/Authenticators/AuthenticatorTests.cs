namespace job_application_tracker.Tests.Business.Authenticators
{
    using job_application_tracker.Business.Authenticators;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Queries.Interfaces;
    using job_application_tracker.Resources;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class AuthenticatorTests : TestBase<Authenticator>
    {
        private readonly User existingUser = new()
        {
            Id = 1,
            Name = "Jordan Hanson",
            Email = "jordan@example.com",
            PasswordHash = "hashed_Password123!"
        };

        [Fact]
        public void Authenticate_ReturnsLoginResponseOnValidCredentials()
        {
            // Arrange
            var request = new LoginRequest { Email = "jordan@example.com", Password = "Password123!" };

            this.automocker.GetMock<IGetUserByEmailQuery>()
                .Setup(x => x.Execute(request.Email))
                .Returns(existingUser);

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.Success);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Authenticate(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUser.Id, result.UserId);
            Assert.Equal("Jordan Hanson", result.Name);
            Assert.Equal("jordan@example.com", result.Email);
        }

        [Fact]
        public void Authenticate_ReturnsNullWhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequest { Email = "nobody@example.com", Password = "Password123!" };

            this.automocker.GetMock<IGetUserByEmailQuery>()
                .Setup(x => x.Execute(request.Email))
                .Returns((User?)null);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Authenticate(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ReturnsNullWhenPasswordIsWrong()
        {
            // Arrange
            var request = new LoginRequest { Email = "jordan@example.com", Password = "WrongPassword!" };

            this.automocker.GetMock<IGetUserByEmailQuery>()
                .Setup(x => x.Execute(request.Email))
                .Returns(existingUser);

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.Failed);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Authenticate(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_AcceptsSuccessRehashNeededAsValid()
        {
            // Arrange
            var request = new LoginRequest { Email = "jordan@example.com", Password = "Password123!" };

            this.automocker.GetMock<IGetUserByEmailQuery>()
                .Setup(x => x.Execute(request.Email))
                .Returns(existingUser);

            this.automocker.GetMock<IPasswordHasher<User>>()
                .Setup(x => x.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.SuccessRehashNeeded);

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Authenticate(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUser.Id, result.UserId);
        }

        [Fact]
        public void Authenticate_DoesNotVerifyPasswordWhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequest { Email = "nobody@example.com", Password = "Password123!" };

            this.automocker.GetMock<IGetUserByEmailQuery>()
                .Setup(x => x.Execute(request.Email))
                .Returns((User?)null);

            var sut = this.CreateTestSubject();

            // Act
            sut.Authenticate(request);

            // Assert
            this.automocker.GetMock<IPasswordHasher<User>>()
                .Verify(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}