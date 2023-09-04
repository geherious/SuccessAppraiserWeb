using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GoalRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async public Task DeleteByUser(ClaimsPrincipal claimsPrincipal, int id)
        {
            List<GoalItem> goals = await GetGoalsByUserAsync(claimsPrincipal);

            var filtered = await _dbContext.Goals.FindAsync(id);

            if (filtered != null)
            {
                _dbContext.Goals.Remove(filtered);
                await _dbContext.SaveChangesAsync();
            }

        }

        async public Task<List<GoalItem>> GetGoalsByUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity == null) { return new List<GoalItem>(); }

            return await (from g in _dbContext.Goals
                    where g.User.UserName == claimsPrincipal.Identity.Name
                    select g).Include(g => g.Dates).ThenInclude(d => d.State).ToListAsync();
        }
    }
}
