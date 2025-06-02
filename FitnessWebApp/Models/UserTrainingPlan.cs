using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class UserTrainingPlan
    {
        public int UserTrainingPlanId { get; set; }
        public int UserId { get; set; }
        public int TrainingPlanId { get; set; }
        public DateTime StartDate { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("UserTrainingPlans")]
        public virtual User User { get; set; }
        [ForeignKey("TrainingPlanId")]
        [InverseProperty("UserTrainingPlans")]
        public virtual TrainingPlan TrainingPlan { get; set; }
    }
}