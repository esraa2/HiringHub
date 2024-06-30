using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Candidate.DLL.Models;

namespace Candidate.DLL.DB_Context
{
    public class CandidateContext : DbContext
    {
        public CandidateContext(DbContextOptions<CandidateContext> options) : base(options) { }

        public DbSet<Models.Candidate> Candidates { get; set; }
    }
}
