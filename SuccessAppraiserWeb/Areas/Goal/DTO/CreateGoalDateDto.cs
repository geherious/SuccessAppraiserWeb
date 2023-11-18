namespace SuccessAppraiserWeb.Areas.Goal.DTO
{
    public record CreateGoalDateDto(DateTime Date, string? Comment, int StateId, int GoalId);
}
