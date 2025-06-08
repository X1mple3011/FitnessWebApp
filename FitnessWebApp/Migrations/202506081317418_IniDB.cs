namespace FitnessWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IniDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.BlogPosts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        BlogPostId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogPostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BlogLikes",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.BlogPosts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(maxLength: 20),
                        Height = c.Decimal(precision: 18, scale: 2),
                        Weight = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.TrainingPlans",
                c => new
                    {
                        TrainingPlanId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Duration = c.Int(),
                    })
                .PrimaryKey(t => t.TrainingPlanId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TrainingDays",
                c => new
                    {
                        TrainingDayId = c.Int(nullable: false, identity: true),
                        PlanId = c.Int(nullable: false),
                        DayNumber = c.Int(nullable: false),
                        Exercise_ExerciseId = c.Int(),
                    })
                .PrimaryKey(t => t.TrainingDayId)
                .ForeignKey("dbo.Exercises", t => t.Exercise_ExerciseId)
                .ForeignKey("dbo.TrainingPlans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId)
                .Index(t => t.Exercise_ExerciseId);
            
            CreateTable(
                "dbo.TrainingDayExercises",
                c => new
                    {
                        TrainingDayExerciseId = c.Int(nullable: false, identity: true),
                        TrainingDayId = c.Int(nullable: false),
                        ExerciseId = c.Int(nullable: false),
                        Sets = c.Int(nullable: false),
                        Repetitions = c.Int(nullable: false),
                        RestTime = c.Int(nullable: false),
                        Notes = c.String(),
                        TrainingPlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainingDayExerciseId)
                .ForeignKey("dbo.Exercises", t => t.ExerciseId)
                .ForeignKey("dbo.TrainingDays", t => t.TrainingDayId)
                .ForeignKey("dbo.TrainingPlans", t => t.TrainingPlanId)
                .Index(t => t.TrainingDayId)
                .Index(t => t.ExerciseId)
                .Index(t => t.TrainingPlanId);
            
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        ExerciseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false),
                        VideoUrl = c.String(maxLength: 500),
                        Equipment = c.String(),
                        Sets = c.Int(nullable: false),
                        Reps = c.Int(nullable: false),
                        RestTimeInSeconds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExerciseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.BlogComments", "PostId", "dbo.BlogPosts");
            DropForeignKey("dbo.BlogPosts", "UserId", "dbo.Users");
            DropForeignKey("dbo.BlogLikes", "UserId", "dbo.Users");
            DropForeignKey("dbo.TrainingPlans", "UserId", "dbo.Users");
            DropForeignKey("dbo.TrainingDays", "PlanId", "dbo.TrainingPlans");
            DropForeignKey("dbo.TrainingDayExercises", "TrainingPlanId", "dbo.TrainingPlans");
            DropForeignKey("dbo.TrainingDayExercises", "TrainingDayId", "dbo.TrainingDays");
            DropForeignKey("dbo.TrainingDayExercises", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.TrainingDays", "Exercise_ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.BlogLikes", "PostId", "dbo.BlogPosts");
            DropIndex("dbo.TrainingDayExercises", new[] { "TrainingPlanId" });
            DropIndex("dbo.TrainingDayExercises", new[] { "ExerciseId" });
            DropIndex("dbo.TrainingDayExercises", new[] { "TrainingDayId" });
            DropIndex("dbo.TrainingDays", new[] { "Exercise_ExerciseId" });
            DropIndex("dbo.TrainingDays", new[] { "PlanId" });
            DropIndex("dbo.TrainingPlans", new[] { "UserId" });
            DropIndex("dbo.BlogLikes", new[] { "UserId" });
            DropIndex("dbo.BlogLikes", new[] { "PostId" });
            DropIndex("dbo.BlogPosts", new[] { "UserId" });
            DropIndex("dbo.BlogComments", new[] { "UserId" });
            DropIndex("dbo.BlogComments", new[] { "PostId" });
            DropTable("dbo.Exercises");
            DropTable("dbo.TrainingDayExercises");
            DropTable("dbo.TrainingDays");
            DropTable("dbo.TrainingPlans");
            DropTable("dbo.Users");
            DropTable("dbo.BlogLikes");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.BlogComments");
        }
    }
}
