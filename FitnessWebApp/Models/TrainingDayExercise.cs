using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class TrainingDayExercise
    {
        [Key]
        public int TrainingDayExerciseId { get; set; }

        [Required]
        public int TrainingDayId { get; set; }
        [ForeignKey("TrainingDayId")]
        public virtual TrainingDay TrainingDay { get; set; }

        [Required]
        public int ExerciseId { get; set; }
        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số set phải lớn hơn 0")]
        [Required]
        public int Sets { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lần lặp lại phải lớn hơn 0")]
        [Required]
        public int Repetitions { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Thời gian nghỉ không được âm")]
        public int RestTime { get; set; }

        public string Notes { get; set; }

        [Required]
        public int TrainingPlanId { get; set; }
        [ForeignKey("TrainingPlanId")]
        public virtual TrainingPlan TrainingPlan { get; set; }
    }
}