using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class BlogLike
    {
        public int BlogLikeId { get; set; }
        public int BlogPostId { get; set; }
        [ForeignKey("BlogPostId")]
        [InverseProperty("BlogLikes")]
        public virtual BlogPost BlogPost { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BlogLikes")]
        public virtual User User { get; set; }
    }
}