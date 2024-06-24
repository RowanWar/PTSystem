using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PTManagementSystem.Models;

namespace PTManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PTManagementSystem.Models.UserModel> UserModel { get; set; } = default!;
        public DbSet<PTManagementSystem.Models.ClientModel> ClientModel { get; set; } = default!;
        public DbSet<PTManagementSystem.Models.WorkoutModel> WorkoutModel { get; set; } = default!;
        public DbSet<PTManagementSystem.Models.ClientWeeklyReportModel> ClientWeeklyReportModel { get; set; } = default!;
        public DbSet<PTManagementSystem.Models.CoachModel> CoachModel { get; set; } = default!;
    }
}
