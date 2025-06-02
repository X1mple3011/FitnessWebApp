using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class TrainingPlanExercise
    {
        public int TrainingPlanExerciseId { get; set; }
        public int TrainingPlanId { get; set; }
        public int ExerciseId { get; set; }
        [ForeignKey("TrainingPlanId")]
        [InverseProperty("TrainingPlanExercises")]
        public virtual TrainingPlan TrainingPlan { get; set; }
        [ForeignKey("ExerciseId")]
        [InverseProperty("TrainingPlanExercises")]
        public virtual Exercise Exercise { get; set; }
    }
}