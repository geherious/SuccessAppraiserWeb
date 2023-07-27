using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;

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
            GoalItem? goal =  _dbContext.Goals.FirstOrDefault(g => g.Id == Id);
            if (goal != null)
            {
                _dbContext.Goals.Remove(goal);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Goal with given Id doesn't exist");
            }
        }
    }
}
