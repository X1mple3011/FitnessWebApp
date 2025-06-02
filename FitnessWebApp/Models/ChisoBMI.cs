using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessWebApp.Models
{
    public class ChiSoBMI
    {
        [Key]
        public int ChiSoBMIId { get; set; }

        public int UserId { get; set; }

        public double BMI { get; set; }

        public DateTime Date { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        [InverseProperty("ChiSoBMIs")]
        public virtual User User { get; set; }

        public ChiSoBMI()
        {
            Date = DateTime.Now;
        }
    }
}
