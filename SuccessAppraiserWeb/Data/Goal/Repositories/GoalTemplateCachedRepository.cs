 using Microsoft.Extensions.Caching.Memory;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Repositories
{
    public class GoalTemplateCachedRepository : IGoalTemplateRepository
    {
        private readonly GoalTemplateRepository _repository;
        private readonly IMemoryCache _cache;

        public GoalTemplateCachedRepository(GoalTemplateRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<GoalTemplate>> GetSystemTemplatesAsync()
        {
            string key = "template-system";
            return await _cache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(2);
                    return _repository.GetSystemTemplatesAsync();
                }) ?? new List<GoalTemplate>();
        }

        public async Task<List<GoalTemplate>> GetUserTemplatesAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity == null) return new List<GoalTemplate>();

            return await _repository.GetUserTemplatesAsync(claimsPrincipal);
        }
    }
}
