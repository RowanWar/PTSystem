using PTManagementSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Models
{
    public class WorkoutExerciseModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public int UserId { get; set; }

        public List<WorkoutExercise>? WorkoutExercises { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan Duration { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public Boolean WorkoutActive { get; set; }

    }


}
