using SuccessAppraiserWeb.Areas.Goal.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Interfaces
{
    public interface IGoalTemplateRepository
    {
        Task<List<GoalTemplate>> GetSystemTemplatesAsync();
        Task<List<GoalTemplate>> GetUserTemplatesAsync(ClaimsPrincipal claimsPrincipal);
    }
}
