using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessWebApp.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }
        
        [Required(ErrorMessage = "Tên bài tập không được để trống")]
        [StringLength(100, ErrorMessage = "Tên bài tập không được vượt quá 100 ký tự")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }
        
        [StringLength(500, ErrorMessage = "URL video không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL video không hợp lệ")]
        public string VideoUrl { get; set; }
        
        public string Equipment { get; set; }
        
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int RestTimeInSeconds { get; set; } // Thời gian nghỉ giữa các set
        
        public virtual ICollection<TrainingDay> TrainingDays { get; set; }
    }
}