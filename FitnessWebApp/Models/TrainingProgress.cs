using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessWebApp.Models
{
    public class TrainingProgress
    {
        [Key]
        public int TrainingProgressId { get; set; }
        public int TrainingPlanRegistrationId { get; set; }
        public int DayNumber { get; set; }
        public bool IsCompleted { get; set; }

        public virtual TrainingPlanRegistration TrainingPlanRegistration { get; set; }
    }

}