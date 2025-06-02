using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserTrainingPlan> UserTrainingPlans { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BlogLike> BlogLikes { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ChiSoBMI> ChiSoBMIs { get; set; }
    }

} 


