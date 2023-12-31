﻿using SuccessAppraiserWeb.Areas.Goal.models;
using System.Security.Claims;

namespace SuccessAppraiserWeb.Data.Goal.Interfaces
{
    public interface IGoalRepository
    {
        Task DeleteByUser(ClaimsPrincipal claimsPrincipal, int Id, CancellationToken cancellationToken = default);

        Task<List<GoalItem>> GetGoalsByUserAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);

        Task<bool> UserHasGoal(ClaimsPrincipal claimsPrincipal, int goalId,CancellationToken cancellationToken = default);
        Task<bool> UserGoalHasDate(ClaimsPrincipal claimsPrincipal, int goalId, DateTime date,  CancellationToken cancellationToken = default);
    }
}
