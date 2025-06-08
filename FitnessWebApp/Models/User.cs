using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessWebApp.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public decimal? Height { get; set; } // Chiều cao (cm)

        public decimal? Weight { get; set; } // Cân nặng (kg)

        // Navigation properties
        public virtual ICollection<TrainingPlan> TrainingPlans { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<BlogLike> BlogLikes { get; set; }
    }
}