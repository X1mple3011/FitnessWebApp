using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<TrainingDay> TrainingDays { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogLike> BlogLikes { get; set; }
        public DbSet<TrainingDayExercise> TrainingDayExercises { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId)
                .Property(u => u.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasColumnName("Username")
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasColumnName("Password")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasColumnName("FullName")
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);

            // TrainingPlan configuration
            modelBuilder.Entity<TrainingPlan>()
                .HasKey(tp => tp.TrainingPlanId)
                .Property(tp => tp.TrainingPlanId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<TrainingPlan>()
                .Property(tp => tp.UserId)
                .HasColumnName("UserId");

            modelBuilder.Entity<TrainingPlan>()
                .HasRequired(tp => tp.User)
                .WithMany(u => u.TrainingPlans)
                .HasForeignKey(tp => tp.UserId)
                .WillCascadeOnDelete(false);

            // Exercise configuration
            modelBuilder.Entity<Exercise>()
                .HasKey(e => e.ExerciseId)
                .Property(e => e.ExerciseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Exercise>()
                .Property(e => e.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Exercise>()
                .Property(e => e.Description)
                .HasColumnName("Description")
                .IsRequired();

            modelBuilder.Entity<Exercise>()
                .Property(e => e.VideoUrl)
                .HasColumnName("VideoUrl")
                .HasMaxLength(500);

            // TrainingDay configuration
            modelBuilder.Entity<TrainingDay>()
                .HasKey(td => td.TrainingDayId)
                .Property(td => td.TrainingDayId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<TrainingDay>()
                .Property(td => td.TrainingPlanId)
                .HasColumnName("PlanId");

            modelBuilder.Entity<TrainingDay>()
                .HasRequired(td => td.TrainingPlan)
                .WithMany(tp => tp.TrainingDays)
                .HasForeignKey(td => td.TrainingPlanId)
                .WillCascadeOnDelete(true);

            // BlogPost configuration
            modelBuilder.Entity<BlogPost>()
                .HasKey(bp => bp.BlogPostId)
                .Property(bp => bp.BlogPostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<BlogPost>()
                .Property(bp => bp.UserId)
                .HasColumnName("UserId");

            modelBuilder.Entity<BlogPost>()
                .HasRequired(bp => bp.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogPost>()
                .Property(bp => bp.Title)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<BlogPost>()
                .Property(bp => bp.Content)
                .HasColumnName("Content")
                .IsRequired();

            // BlogComment configuration
            modelBuilder.Entity<BlogComment>()
                .HasKey(bc => bc.CommentId)
                .Property(bc => bc.CommentId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<BlogComment>()
                .Property(bc => bc.BlogPostId)
                .HasColumnName("PostId");

            modelBuilder.Entity<BlogComment>()
                .Property(bc => bc.UserId)
                .HasColumnName("UserId");

            modelBuilder.Entity<BlogComment>()
                .HasRequired(bc => bc.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(bc => bc.BlogPostId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogComment>()
                .HasRequired(bc => bc.User)
                .WithMany(u => u.BlogComments)
                .HasForeignKey(bc => bc.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogComment>()
                .Property(bc => bc.Content)
                .HasColumnName("Content")
                .IsRequired();

            // BlogLike configuration
            modelBuilder.Entity<BlogLike>()
                .HasKey(bl => bl.LikeId)
                .Property(bl => bl.LikeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<BlogLike>()
                .Property(bl => bl.BlogPostId)
                .HasColumnName("PostId");

            modelBuilder.Entity<BlogLike>()
                .Property(bl => bl.UserId)
                .HasColumnName("UserId");

            modelBuilder.Entity<BlogLike>()
                .HasRequired(bl => bl.BlogPost)
                .WithMany(bp => bp.Likes)
                .HasForeignKey(bl => bl.BlogPostId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogLike>()
                .HasRequired(bl => bl.User)
                .WithMany(u => u.BlogLikes)
                .HasForeignKey(bl => bl.UserId)
                .WillCascadeOnDelete(false);

            // TrainingDayExercise configuration
            modelBuilder.Entity<TrainingDayExercise>()
                .HasKey(tde => tde.TrainingDayExerciseId)
                .Property(tde => tde.TrainingDayExerciseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<TrainingDayExercise>()
                .Property(tde => tde.TrainingDayId)
                .HasColumnName("TrainingDayId");

            modelBuilder.Entity<TrainingDayExercise>()
                .Property(tde => tde.ExerciseId)
                .HasColumnName("ExerciseId");

            modelBuilder.Entity<TrainingDayExercise>()
                .HasRequired(tde => tde.TrainingDay)
                .WithMany(td => td.TrainingDayExercises)
                .HasForeignKey(tde => tde.TrainingDayId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TrainingDayExercise>()
                .HasRequired(tde => tde.Exercise)
                .WithMany()
                .HasForeignKey(tde => tde.ExerciseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TrainingDayExercise>()
                .HasRequired(tde => tde.TrainingPlan)
                .WithMany()
                .HasForeignKey(tde => tde.TrainingPlanId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
