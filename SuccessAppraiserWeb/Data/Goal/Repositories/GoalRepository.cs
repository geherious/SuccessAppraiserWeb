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

        async public Task DeleteByUser(ClaimsPrincipal claimsPrincipal, int id, CancellationToken cancellationToken)
        {
            List<GoalItem> goals = await GetGoalsByUserAsync(claimsPrincipal, cancellationToken);

            var filtered = await _dbContext.Goals.FindAsync(id, cancellationToken);

            if (filtered != null)
            {
                _dbContext.Goals.Remove(filtered);
                await _dbContext.SaveChangesAsync();
            }

        }

        async public Task<List<GoalItem>> GetGoalsByUserAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            if (claimsPrincipal.Identity == null) { return new List<GoalItem>(); }

            return await (from g in _dbContext.Goals
                    where g.User.UserName == claimsPrincipal.Identity.Name
                    select g)
                    .Include(g => g.Template).ThenInclude(t => t.States)
                    .Include(g => g.Dates.OrderBy(item => item.Date))
                    .ThenInclude(d => d.State).ToListAsync(cancellationToken);
        }
    }
}
