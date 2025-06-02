using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [InverseProperty("Exercise")]
        public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; }
    }
}