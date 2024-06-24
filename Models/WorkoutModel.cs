using PTManagementSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Models
{
    public class WorkoutModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public int UserId { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan WorkoutDuration { get; set; }

        public DateTime WorkoutCreatedAt { get; set; }

    }
}
