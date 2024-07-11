using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Models
{
    public class WorkoutExercisesModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public int UserId { get; set; }

        public int ExerciseGroupId { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan WorkoutDuration { get; set; }

        public DateTime WorkoutCreatedAt { get; set; }

        public string ExerciseName { get; set; }

        public string MuscleGroup { get; set; }

        public string ExerciseDescription { get; set; }

        public int SetId { get; set; }

        public int SetReps { get; set; }

        public decimal SetWeight { get; set; }

        public string SetCategory { get; set; }
    }
}
