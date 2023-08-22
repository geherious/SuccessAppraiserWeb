﻿using SuccessAppraiserWeb.Areas.Identity.models;
using System.ComponentModel.DataAnnotations;

namespace SuccessAppraiserWeb.Areas.Goal.models
{
    public class GoalTemplate
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser? User { get; set; }
        public int? UserId { get; set; }
        [Required]
        public List<GoalState> States { get; set; } = new List<GoalState>();
    }
}