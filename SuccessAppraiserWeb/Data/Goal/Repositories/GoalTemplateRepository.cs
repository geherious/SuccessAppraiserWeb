using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Repositories
{
    public class GoalTemplateRepository : IGoalTemplateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GoalTemplateRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<GoalTemplate>> GetSystemTemplatesAsync()
        {
            return await (from g in _dbContext.GoalTemplates
                          where g.User == null
                          orderby g.Name
                          select g).ToListAsync();
        }

        public async Task<List<GoalTemplate>> GetUserTemplatesAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity == null) return new List<GoalTemplate>();

            return await (from g in _dbContext.GoalTemplates
                          where g.User.UserName == claimsPrincipal.Identity.Name
                          select g).ToListAsync();
        }
    }
}
