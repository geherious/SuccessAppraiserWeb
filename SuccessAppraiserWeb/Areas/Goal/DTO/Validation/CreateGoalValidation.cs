using FluentValidation;

namespace SuccessAppraiserWeb.Areas.Goal.DTO.Validation
{
    public class CreateGoalValidation : AbstractValidator<CreateGoalDto>
    {
        public CreateGoalValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(48);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.DaysNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.TemplateId)
                .NotEmpty();

            RuleFor(x => x.DateStart)
                .NotEmpty();
        }
    }
}
