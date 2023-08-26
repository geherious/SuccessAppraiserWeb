using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using SuccessAppraiserWeb.Areas.Identity.models;
using System.Text.Json.Serialization;

namespace SuccessAppraiserWeb.Areas.Goal.models
{
    public class GoalItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(48)]
        public string? Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [Required]
        public int DaysNumber { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateStart { get; set; }
        public List<GoalDate> Dates { get; set; } = new List<GoalDate>();

        [Required]
        public ApplicationUser? User { get; set; }

        //[Required]
        //public int? TemplateId { get; set; }
        //[Required]
        //public GoalTemplate? Template { get; set; }


    }
}
