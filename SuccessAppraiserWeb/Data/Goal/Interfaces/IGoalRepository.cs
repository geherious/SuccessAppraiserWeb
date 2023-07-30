using SuccessAppraiserWeb.Areas.Goal.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Interfaces
{
    public interface IGoalRepository
    {
        void Delete(int Id);

        void Delete(ClaimsPrincipal claimsPrincipal, int Id);

        List<GoalItem>? GetGoalsByUser(ClaimsPrincipal claimsPrincipal);
    }
}
