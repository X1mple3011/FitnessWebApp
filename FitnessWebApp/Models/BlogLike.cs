using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class BlogLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public int BlogPostId { get; set; }
        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}