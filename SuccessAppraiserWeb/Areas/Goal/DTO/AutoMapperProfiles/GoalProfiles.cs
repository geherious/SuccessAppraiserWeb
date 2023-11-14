using AutoMapper;
using SuccessAppraiserWeb.Areas.Goal.models;

namespace SuccessAppraiserWeb.Areas.Goal.DTO.AutoMapperProfiles
{
    public class GoalProfiles : Profile
    {
        public GoalProfiles()
        {
            CreateMap<GoalItem, CreateGoalDto>().ReverseMap();
        }
    }
}
