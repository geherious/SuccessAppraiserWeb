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
        public void Delete(int Id)
        {
            GoalItem? goal =  _dbContext.Goals.Find(Id);
            if (goal != null)
            {
                _dbContext.Goals.Remove(goal);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteByUser(ClaimsPrincipal claimsPrincipal, int Id)
        {
            List<GoalItem>? goals = GetGoalsByUser(claimsPrincipal);
            var filtered = (from g in _dbContext.Goals
                            where g.Id == Id
                            select g).ToList();

            if (filtered.Count == 1)
            {
                _dbContext.Goals.Remove(filtered.First());
                _dbContext.SaveChanges();
            }

        }

        public List<GoalItem>? GetGoalsByUser(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity == null) { return null; }

            return (from g in _dbContext.Goals
                    where g.User.UserName == claimsPrincipal.Identity.Name
                    select g).Include(g => g.Dates).ThenInclude(d => d.State).ToList();
        }
    }
}
