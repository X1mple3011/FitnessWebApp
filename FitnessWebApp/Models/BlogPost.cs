using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class BlogPost
    {
        public int BlogPostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BlogPosts")]
        public virtual User User { get; set; }
        [InverseProperty("BlogPost")]
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        [InverseProperty("BlogPost")]
        public virtual ICollection<BlogLike> BlogLikes { get; set; }
    }
}