using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PTManagementSystem.Models
{
    public class WorkoutExercisesModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public int UserId { get; set; }

        public int ExerciseGroupId { get; set; }

        public int ExerciseId { get; set; }

        public int WorkoutExerciseId { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }

        public Boolean WorkoutActive{ get; set; }

        public string WorkoutName { get; set; }

        public string ExerciseName { get; set; }

        public string MuscleGroup { get; set; }

        /*stores a list of muscle groups for a dropdown list in the view. List options stored directly in the controller*/
        //public IEnumerable<SelectListItem> MuscleGroupList { get; set; } 

        public string ExerciseDescription { get; set; }

        public int SetId { get; set; }

        public Int64 SetsCount { get; set; }

        public int Reps { get; set; }

        public decimal Weight { get; set; }

        // Used to display an array of weight per set to the user in the front-end from CheckForActiveWeight() query.
        public Array WeightPerSet { get; set; }

        public int SetCategory { get; set; }

        public string SetCategoryAsString { get; set; }

        public DateTime starttime { get; set; }

        public DateTime endtime { get; set; }

        //public IEnumerable<SelectListItem> SetCategoryList { get; set; }
    }
}
