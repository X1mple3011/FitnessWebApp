using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class TrainingPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainingPlanId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên kế hoạch không được để trống")]
        [StringLength(100, ErrorMessage = "Tên kế hoạch không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số ngày tập phải lớn hơn 0")]
        public int? Duration { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<TrainingDay> TrainingDays { get; set; }
    }
}