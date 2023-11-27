using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.Exceptions;
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

        public async Task DeleteByUser(ClaimsPrincipal claimsPrincipal, int id, CancellationToken cancellationToken)
        {
            List<GoalItem> goals = await GetGoalsByUserAsync(claimsPrincipal, cancellationToken);

            var filtered = await _dbContext.Goals.FindAsync(id, cancellationToken);

            if (filtered != null)
            {
                _dbContext.Goals.Remove(filtered);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<List<GoalItem>> GetGoalsByUserAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            if (claimsPrincipal.Identity == null) { return new List<GoalItem>(); }

            return await (from g in _dbContext.Goals
                    where g.User.UserName == claimsPrincipal.Identity.Name
                    select g)
                    .Include(g => g.Template).ThenInclude(t => t.States)
                    .Include(g => g.Dates.OrderBy(item => item.Date))
                    .ThenInclude(d => d.State).ToListAsync(cancellationToken);
        }

        public async Task<bool> UserHasGoal(ClaimsPrincipal claimsPrincipal, int goalId, CancellationToken cancellationToken)
        {
            if (claimsPrincipal.Identity == null) { return false; }

            var goal = await _dbContext.Goals.
                Where(g => g.Id == goalId && g.User.UserName == claimsPrincipal.Identity.Name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return goal != null;
        }

        public async Task<bool> UserGoalHasDate(ClaimsPrincipal claimsPrincipal, int goalId, DateTime date,
            CancellationToken cancellationToken = default)
        {
            var goal = await _dbContext.Goals.
                Where(g => g.Id == goalId && g.User.UserName == claimsPrincipal.Identity.Name)
                .Include(g => g.Dates)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (goal == null) throw new UserHasNoGoalException("Trying to access user's goal, but it doesn't exist");
            var dateCount = goal.Dates.Count(d => d.Date.Date == date.Date);
            return dateCount == 1;
        }

    }
}
