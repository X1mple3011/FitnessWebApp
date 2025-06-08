using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessWebApp.Models
{
    public class TrainingPlanRegistration
    {
        [Key]
        public int TrainingPlanRegistrationId { get; set; }
        public int UserId { get; set; }
        public int TrainingPlanId { get; set; }
        public DateTime StartDate { get; set; }

        public virtual ICollection<TrainingProgress> TrainingProgresses { get; set; }
        public virtual User User { get; set; }
        public virtual TrainingPlan TrainingPlan { get; set; }
    }



}