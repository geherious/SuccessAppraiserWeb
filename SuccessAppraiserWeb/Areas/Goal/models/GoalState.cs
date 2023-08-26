using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuccessAppraiserWeb.Areas.Goal.models
{
    public class GoalState
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [RegularExpression(@"(?i)[0-9A-Z]{6}")]
        public string? Color { get; set; }
        public List<GoalTemplate> Templates { get; set; } = new List<GoalTemplate>();
    }
}
