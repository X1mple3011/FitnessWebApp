using System.Data.Entity;
using FitnessWebApp.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TrainingPlan> TrainingPlans { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<TrainingPlanExercise> TrainingPlanExercises { get; set; }
        public virtual DbSet<UserTrainingPlan> UserTrainingPlans { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<BlogComment> BlogComments { get; set; }
        public virtual DbSet<BlogLike> BlogLikes { get; set; }
        public virtual DbSet<ChiSoBMI> ChiSoBMIs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .WillCascadeOnDelete(false);

            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);
            modelBuilder.Entity<Role>()
                .Property(r => r.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // TrainingPlan
            modelBuilder.Entity<TrainingPlan>()
                .HasKey(tp => tp.TrainingPlanId);
            modelBuilder.Entity<TrainingPlan>()
                .Property(tp => tp.TrainingPlanId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Exercise
            modelBuilder.Entity<Exercise>()
                .HasKey(e => e.ExerciseId);
            modelBuilder.Entity<Exercise>()
                .Property(e => e.ExerciseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // TrainingPlanExercise (many-to-many)
            modelBuilder.Entity<TrainingPlanExercise>()
                .HasKey(tpe => tpe.TrainingPlanExerciseId);
            modelBuilder.Entity<TrainingPlanExercise>()
                .Property(tpe => tpe.TrainingPlanExerciseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TrainingPlanExercise>()
                .HasRequired(tpe => tpe.TrainingPlan)
                .WithMany(tp => tp.TrainingPlanExercises)
                .HasForeignKey(tpe => tpe.TrainingPlanId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<TrainingPlanExercise>()
                .HasRequired(tpe => tpe.Exercise)
                .WithMany(e => e.TrainingPlanExercises)
                .HasForeignKey(tpe => tpe.ExerciseId)
                .WillCascadeOnDelete(false);

            // UserTrainingPlan (many-to-many)
            modelBuilder.Entity<UserTrainingPlan>()
                .HasKey(utp => utp.UserTrainingPlanId);
            modelBuilder.Entity<UserTrainingPlan>()
                .Property(utp => utp.UserTrainingPlanId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserTrainingPlan>()
                .HasRequired(utp => utp.User)
                .WithMany(u => u.UserTrainingPlans)
                .HasForeignKey(utp => utp.UserId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<UserTrainingPlan>()
                .HasRequired(utp => utp.TrainingPlan)
                .WithMany(tp => tp.UserTrainingPlans)
                .HasForeignKey(utp => utp.TrainingPlanId)
                .WillCascadeOnDelete(false);

            // BlogPost
            modelBuilder.Entity<BlogPost>()
                .HasKey(bp => bp.BlogPostId);
            modelBuilder.Entity<BlogPost>()
                .Property(bp => bp.BlogPostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BlogPost>()
                .HasRequired(bp => bp.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.UserId)
                .WillCascadeOnDelete(false);

            // BlogComment
            modelBuilder.Entity<BlogComment>()
                .HasKey(bc => bc.BlogCommentId);
            modelBuilder.Entity<BlogComment>()
                .Property(bc => bc.BlogCommentId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BlogComment>()
                .HasRequired(bc => bc.BlogPost)
                .WithMany(bp => bp.BlogComments)
                .HasForeignKey(bc => bc.BlogPostId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<BlogComment>()
                .HasRequired(bc => bc.User)
                .WithMany(u => u.BlogComments)
                .HasForeignKey(bc => bc.UserId)
                .WillCascadeOnDelete(false);

            // BlogLike
            modelBuilder.Entity<BlogLike>()
                .HasKey(bl => bl.BlogLikeId);
            modelBuilder.Entity<BlogLike>()
                .Property(bl => bl.BlogLikeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BlogLike>()
                .HasRequired(bl => bl.BlogPost)
                .WithMany(bp => bp.BlogLikes)
                .HasForeignKey(bl => bl.BlogPostId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<BlogLike>()
                .HasRequired(bl => bl.User)
                .WithMany(u => u.BlogLikes)
                .HasForeignKey(bl => bl.UserId)
                .WillCascadeOnDelete(false);

            // ChiSoBMI
            modelBuilder.Entity<ChiSoBMI>()
                .HasKey(bmi => bmi.ChiSoBMIId);
            modelBuilder.Entity<ChiSoBMI>()
                .Property(bmi => bmi.ChiSoBMIId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ChiSoBMI>()
                .HasRequired(bmi => bmi.User)
                .WithMany(u => u.ChiSoBMIs)
                .HasForeignKey(bmi => bmi.UserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
} 