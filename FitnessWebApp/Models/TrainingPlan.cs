using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class TrainingPlan
    {
        public int TrainingPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MinBMI { get; set; }
        public double MaxBMI { get; set; }
        [InverseProperty("TrainingPlan")]
        public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; }
        [InverseProperty("TrainingPlan")]
        public virtual ICollection<UserTrainingPlan> UserTrainingPlans { get; set; }
    }
}