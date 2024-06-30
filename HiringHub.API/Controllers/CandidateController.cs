using HiringHub.BLL.Services;
using HiringHub.DLL.DB_Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HiringHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly CandidateService _candidateService;

        public CandidateController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCandidates()
        {
            var candidates =  _candidateService.GetAllCandidates();
            return Ok(candidates);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCandidate(HiringHub.DLL.Models.Candidate candidate)
        {
            try
            {
                await _candidateService.UpsertCandidate(candidate);
                return Ok(candidate);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
