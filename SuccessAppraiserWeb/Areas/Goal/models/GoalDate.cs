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
        [Required]
        public GoalState? State { get; set; }

        [Required]
        public int GoalId { get; set; }
        [Required]
        public GoalItem? Goal { get; set; }

    }
}
