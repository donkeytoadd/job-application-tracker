namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class UpdateApplicationStatusQueryTests : TestBase<UpdateApplicationStatusQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private int SeedApplication(DbContextOptions<ApplicationDbContext> options, ApplicationStatus initialStatus = ApplicationStatus.Applied)
        {
            using var context = new ApplicationDbContext(options);
            var application = new JobApplication
            {
                UserId = 1,
                Company = "Google",
                Role = "Senior Engineer",
                Status = initialStatus,
                StatusHistory = new List<ApplicationStatusHistory>
                {
                    new() { Status = initialStatus }
                }
            };
            context.JobApplication.Add(application);
            context.SaveChanges();
            return application.Id;
        }

        [Fact]
        public void Execute_UpdatesApplicationStatus()
        {
            // Arrange
            var options = CreateOptions();
            var applicationId = SeedApplication(options);

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            sut.Execute(applicationId, ApplicationStatus.Screening);

            // Assert
            using var verifyContext = new ApplicationDbContext(options);
            var updated = verifyContext.JobApplication.Find(applicationId);
            Assert.Equal(ApplicationStatus.Screening, updated!.Status);
        }

        [Fact]
        public void Execute_AppendsStatusHistoryEntry()
        {
            // Arrange
            var options = CreateOptions();
            var applicationId = SeedApplication(options);

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            sut.Execute(applicationId, ApplicationStatus.Screening);

            // Assert
            using var verifyContext = new ApplicationDbContext(options);
            var history = verifyContext.ApplicationStatusHistory
                .Where(h => h.ApplicationId == applicationId)
                .ToList();

            Assert.Equal(2, history.Count);
            Assert.Contains(history, h => h.Status == ApplicationStatus.Applied);
            Assert.Contains(history, h => h.Status == ApplicationStatus.Screening);
        }

        [Fact]
        public void Execute_ReturnsUpdatedApplicationWithStatusHistory()
        {
            // Arrange
            var options = CreateOptions();
            var applicationId = SeedApplication(options);

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(applicationId, ApplicationStatus.Interview);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationStatus.Interview, result.Status);
            Assert.Equal(2, result.StatusHistory.Count);
        }

        [Fact]
        public void Execute_ReturnsNullWhenApplicationNotFound()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(99, ApplicationStatus.Screening);

            // Assert
            Assert.Null(result);
        }
    }
}