using Candidate.BLL.Services;
using Candidate.DLL.DB_Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandidateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly CandidateContext _context;

        public CandidateController(CandidateContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCandidate(Candidate.DLL.Models.Candidate candidate)
        {
            CandidateService service = new CandidateService(_context); 
            await service.UpsertCandidate(candidate);
            return Ok(candidate);
        }
       
    }
}
