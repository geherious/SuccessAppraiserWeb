using SuccessAppraiserWeb.Areas.Goal.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Interfaces
{
    public interface IGoalRepository
    {
        Task DeleteByUser(ClaimsPrincipal claimsPrincipal, int Id);

        Task<List<GoalItem>> GetGoalsByUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
