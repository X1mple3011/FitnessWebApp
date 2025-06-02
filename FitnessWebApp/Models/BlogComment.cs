using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class BlogComment
    {
        public int BlogCommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogPostId { get; set; }
        [ForeignKey("BlogPostId")]
        [InverseProperty("BlogComments")]
        public virtual BlogPost BlogPost { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BlogComments")]
        public virtual User User { get; set; }
    }
}