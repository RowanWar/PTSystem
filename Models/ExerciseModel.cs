﻿using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Models
{
    public class ExerciseModel
    {
        [Key]
        public int ExerciseId {  get; set; }

        public string ExerciseName { get; set; }

        public string MuscleGroup { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public int? UserId { get; set; }
    }
}
