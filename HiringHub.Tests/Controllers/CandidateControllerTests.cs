using Moq;
using Microsoft.AspNetCore.Mvc;
using HiringHub.Controllers;
using HiringHub.BLL.Services;
using HiringHub.DLL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace HiringHub.Tests.Controllers
{
    [TestFixture]
    public class CandidateControllerTests
    {
        private Mock<IRepository<HiringHub.DLL.Models.Candidate>> _candidateRepositoryMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private CandidateService _candidateService;
        private CandidateController _controller;

        [SetUp]
        public void Setup()
        {
            _candidateRepositoryMock = new Mock<IRepository<HiringHub.DLL.Models.Candidate>>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _candidateService = CreateCandidateService(_candidateRepositoryMock.Object, _memoryCacheMock.Object);
            _controller = new CandidateController(_candidateService);
        }
        public CandidateService CreateCandidateService(IRepository<HiringHub.DLL.Models.Candidate> candidateRepository, IMemoryCache cache)
        {
            return new CandidateService(candidateRepository, cache);
        }

        [Test]
        public async Task UpsertCandidate_ValidCandidate_ReturnsOkResult()
        {
            // Arrange
            var candidate = new HiringHub.DLL.Models.Candidate { Id = 1, FirstName = "Esraa", LastName = "Ali", Email = "esraashaabanali.94@gmail.com" };

            _candidateRepositoryMock.Setup(repo => repo.Update(candidate));

            // Act
            var result = await _controller.UpsertCandidate(candidate);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(result);

            var returnedCandidate = okResult.Value as HiringHub.DLL.Models.Candidate;
            Assert.IsNotNull(returnedCandidate);
            Assert.AreEqual(candidate.Id, returnedCandidate.Id);
        }

        [Test]
        public async Task UpsertCandidate_InvalidCandidate_ReturnsBadRequest()
        {
            // Arrange
            var invalidCandidate = new HiringHub.DLL.Models.Candidate { Id = 1, FirstName = "Esraa", LastName = "Ali", Email = "invalid email" };
            _controller.ModelState.AddModelError("Email", "Invalid email format.");

            // Act
            var result = await _controller.UpsertCandidate(invalidCandidate);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            Assert.AreEqual("Validation failed", badRequestResult.Value.GetType().GetProperties()[0].GetValue(badRequestResult.Value));

            Assert.AreEqual("Invalid email format.", badRequestResult.Value.GetType().GetProperties()[1].GetValue(badRequestResult.Value));
        }
    }
}
