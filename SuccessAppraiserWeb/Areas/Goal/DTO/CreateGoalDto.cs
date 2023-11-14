namespace SuccessAppraiserWeb.Areas.Goal.DTO
{
    public record CreateGoalDto(string Name, string Description, int DaysNumber, DateTime DateStart, int TemplateId);

}
