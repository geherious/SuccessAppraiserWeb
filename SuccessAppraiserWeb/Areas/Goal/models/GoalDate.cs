using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccessAppraiserWeb.Areas.Goal.models
{
    public class GoalDate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        public string? Comment { get; set; }

        [Required]
        public int StateId { get; set; }
        public GoalState GoalState { get; set; }

        [Required]
        public int GoalId { get; set; }
        public GoalItem Goal { get; set; }

    }
}
