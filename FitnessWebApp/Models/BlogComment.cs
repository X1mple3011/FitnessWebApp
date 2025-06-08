using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class BlogComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Nội dung bình luận không được để trống")]
        [StringLength(1000, ErrorMessage = "Nội dung bình luận không được vượt quá 1000 ký tự")]
        public string Content { get; set; }

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