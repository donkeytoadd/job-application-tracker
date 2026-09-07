namespace job_application_tracker.Tests.Data.Queries
{
    using job_application_tracker.Data;
    using job_application_tracker.Data.Entities;
    using job_application_tracker.Data.Enums;
    using job_application_tracker.Data.Queries;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class GetApplicationByIdQueryTests : TestBase<GetApplicationByIdQuery>
    {
        private DbContextOptions<ApplicationDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        [Fact]
        public void Execute_ReturnsMatchingApplication()
        {
            // Arrange
            var options = CreateOptions();

            using (var seedContext = new ApplicationDbContext(options))
            {
                seedContext.JobApplication.AddRange(
                    new JobApplication
                    {
                        UserId = 1, 
                        Company = "Google", 
                        Role = "Engineer"
                    },
                    new JobApplication
                    {
                        UserId = 1, 
                        Company = "Microsoft", 
                        Role = "Developer"
                    });
                
                seedContext.SaveChanges();
            }

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            int targetId;
            using (var context = new ApplicationDbContext(options))
            {
                targetId = context.JobApplication.Single(x => x.Company == "Microsoft").Id;
            }

            // Act
            var result = sut.Execute(targetId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Microsoft", result.Company);
        }

        [Fact]
        public void Execute_ReturnsNullWhenApplicationDoesNotExist()
        {
            // Arrange
            var options = CreateOptions();

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_IncludesStatusHistory()
        {
            // Arrange
            var options = CreateOptions();
            int applicationId;

            using (var seedContext = new ApplicationDbContext(options))
            {
                var application = new JobApplication
                {
                    UserId = 1,
                    Company = "Google",
                    Role = "Engineer",
                    StatusHistory = new List<ApplicationStatusHistory>
                    {
                        new()
                        {
                            Status = ApplicationStatus.Applied
                        },
                        new()
                        {
                            Status = ApplicationStatus.Screening
                        }
                    }
                };
                seedContext.JobApplication.Add(application);
                seedContext.SaveChanges();
                applicationId = application.Id;
            }

            this.automocker.GetMock<IDbContextFactory<ApplicationDbContext>>()
                .Setup(x => x.CreateDbContext())
                .Returns(() => new ApplicationDbContext(options));

            var sut = this.CreateTestSubject();

            // Act
            var result = sut.Execute(applicationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.StatusHistory.Count);
        }
    }
}