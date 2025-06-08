using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessWebApp.Models
{
    public class TrainingDay
    {
        [Key]
        public int TrainingDayId { get; set; }

        [Required]
        public int TrainingPlanId { get; set; }

        [Required]
        public int DayNumber { get; set; }

        // Navigation properties
        public virtual TrainingPlan TrainingPlan { get; set; }
        public virtual ICollection<TrainingDayExercise> TrainingDayExercises { get; set; }
    }
}