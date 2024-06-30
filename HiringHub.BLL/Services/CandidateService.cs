using Candidate.DLL.DB_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.BLL.Services
{
    public class CandidateService
    {
        private readonly CandidateContext _context;
        public CandidateService(CandidateContext context)
        {
            _context = context;
        }
        public async Task UpsertCandidate(Candidate.DLL.Models.Candidate candidate)
        {
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            #region -- Update The existing Candidate
            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.CallTimeInterval = candidate.CallTimeInterval;
                existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
                existingCandidate.Comment = candidate.Comment;
             
                _context.Candidates.Update(existingCandidate);
            }
            #endregion

            #region -- Create New Candidate
            else
            {
                _context.Candidates.Add(candidate);
            }
            #endregion

            await _context.SaveChangesAsync();
        }
    }
}
