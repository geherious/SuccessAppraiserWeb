using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuccessAppraiserWeb.Areas.Goal.models
{
    [Index(nameof(Name), IsUnique = true)]
    public class GoalState
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
