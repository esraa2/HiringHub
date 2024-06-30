using Microsoft.EntityFrameworkCore;

namespace HiringHub.DLL.DB_Context
{
    public class CandidateContext : DbContext
    {
        public CandidateContext(DbContextOptions<CandidateContext> options) : base(options) { }

        public DbSet<Models.Candidate> Candidates { get; set; }
    }
}
