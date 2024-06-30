using HiringHub.BLL.Services;
using HiringHub.DLL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HiringHub.Tests.Services
{
    [TestFixture]
    public class CandidateServiceTests
    {
        private Mock<IRepository<HiringHub.DLL.Models.Candidate>> _candidateRepositoryMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private CandidateService _candidateService;

        [SetUp]
        public void Setup()
        {
            _candidateRepositoryMock = new Mock<IRepository<HiringHub.DLL.Models.Candidate>>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _candidateService = new CandidateService(_candidateRepositoryMock.Object, _memoryCacheMock.Object);
        }

        [Test]
        public void UpsertCandidate_ValidCandidate_AddsOrUpdateCandidate()
        {
            // Arrange
            var validCandidate = new HiringHub.DLL.Models.Candidate
            {
                Id = 1,
                FirstName = "Esraa",
                LastName = "Ali",
                Email = "esraashaabanali.94@gmail.com",
                LinkedInProfileUrl = "https://www.linkedin.com/in/esraa-shaaban-ali-809686191/",
                GitHubProfileUrl = "https://github.com/esraa2/",
                Comment = "Test candidate"
            };

            _candidateRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<HiringHub.DLL.Models.Candidate>().AsQueryable());
            _candidateRepositoryMock.Setup(repo => repo.Add(validCandidate));

            // Act
            _candidateService.UpsertCandidate(validCandidate);

            // assert
            _candidateRepositoryMock.Verify(repo => repo.Add(validCandidate), Times.Once);
            _candidateRepositoryMock.Verify(repo => repo.Update(It.IsAny<HiringHub.DLL.Models.Candidate>()), Times.Never);
            _candidateRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpsertCandidate_InvalidCandidate_ThrowsArgumentException()
        {
            // Arrange
            var invalidCandidate = new HiringHub.DLL.Models.Candidate { Id = 1, FirstName = "Esraa", LastName = "Ali", Email = "invalid-email" };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _candidateService.UpsertCandidate(invalidCandidate));
            Assert.IsTrue(exception.Message.Contains("Invalid candidate data"));
            Assert.IsTrue(exception.Message.Contains("The Email field is not a valid e-mail address"));
        }
    }
}
