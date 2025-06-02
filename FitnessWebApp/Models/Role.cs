using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models { 

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } // "Admin", "User"
        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; }
    } 

} 