using HiringHub.DLL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace HiringHub.BLL.Services
{
    public class CandidateService
    {
        #region -- Fields
        private readonly IRepository<HiringHub.DLL.Models.Candidate> _candidateRepository;
        
        //Cache
        private readonly IMemoryCache _cache;
        private const string CandidateCacheKey = "candidates_cache";
        #endregion

        #region --Constructors
        public CandidateService(IRepository<HiringHub.DLL.Models.Candidate> candidateRepository, IMemoryCache cache)
        {
            _candidateRepository = candidateRepository;
            _cache = cache; 
        }
        #endregion
       
        #region --Methods
        public async Task UpsertCandidate(HiringHub.DLL.Models.Candidate candidate)
        {
            #region -- Testing validation on level of service

            var validationContext = new ValidationContext(candidate, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(candidate, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                var errorMessages = validationResults.Select(vr => vr.ErrorMessage);
                throw new ArgumentException($"Invalid candidate data: {string.Join(", ", errorMessages)}");
            }
            #endregion

            var existingCandidate = _candidateRepository.GetAll().FirstOrDefault(c => c.Email == candidate.Email);

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

                _candidateRepository.Update(existingCandidate);
            }
            #endregion

            #region -- Create New Candidate
            else
            {
                _candidateRepository.Add(candidate);
            }
            #endregion
            try
            {
                _candidateRepository.SaveChanges();
            }
            catch(Exception ex){
                throw ex;
            }
            _cache.Remove(CandidateCacheKey);  //remove cache after add or update
        }

        public  IEnumerable<HiringHub.DLL.Models.Candidate> GetAllCandidates()
        {
            if (!_cache.TryGetValue(CandidateCacheKey, out IEnumerable<HiringHub.DLL.Models.Candidate> candidates))
            {
                candidates =  _candidateRepository.GetAll();
               
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                // Save data in cache
                _cache.Set(CandidateCacheKey, candidates, cacheEntryOptions);
            }

            return candidates;
        }

        #endregion
    }
}
