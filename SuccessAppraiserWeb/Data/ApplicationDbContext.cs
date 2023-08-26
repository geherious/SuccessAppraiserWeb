using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Areas.Identity.models;

namespace SuccessAppraiserWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GoalItem> Goals { get; set; }
        public DbSet<GoalDate> GoalDates { get; set; }
        public DbSet<GoalState> GoalStates { get; set; }
        public DbSet<GoalTemplate> GoalTemplates { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GoalTemplate>()
                .HasIndex(g => g.Name)
                .IsUnique();
            base.OnModelCreating(builder);
        }
    }
}