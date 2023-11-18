using System.Data;
using AutoMapper;
using FluentValidation;

namespace SuccessAppraiserWeb.Areas.Goal.DTO.Validation
{
    public class CreateGoalDateValidation : AbstractValidator<CreateGoalDateDto>
    {
        public CreateGoalDateValidation()
        {
            RuleFor(x => x.Comment)
                .MaximumLength(1024);

            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.GoalId)
                .NotNull();

            RuleFor(x => x.StateId)
                .NotNull();
        }
    }
}
