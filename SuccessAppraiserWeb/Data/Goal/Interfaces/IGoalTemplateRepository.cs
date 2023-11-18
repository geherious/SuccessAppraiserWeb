using SuccessAppraiserWeb.Areas.Goal.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Interfaces
{
    public interface IGoalTemplateRepository
    {
        Task<List<GoalTemplate>> GetSystemTemplatesAsync(CancellationToken cancellationToken = default);
        Task<List<GoalTemplate>> GetUserTemplatesAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);
    }
}
