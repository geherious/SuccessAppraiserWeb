using Microsoft.AspNetCore.Identity;
using SuccessAppraiserWeb.Areas.Goal.models;

namespace SuccessAppraiserWeb.Areas.Identity.models
{
    public class ApplicationUser : IdentityUser
    {
        public List<GoalItem> Goals { get; set; } = new List<GoalItem>(); 
    }
}
