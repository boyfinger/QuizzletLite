using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DAO;

public partial class QuizletLiteContext : DbContext
{
    public QuizletLiteContext(DbContextOptions<QuizletLiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizResult> QuizResults { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SelectedAnswer> SelectedAnswers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SelectedAnswer>()
            .HasOne(sa => sa.Question)
            .WithMany(q => q.SelectedAnswers)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SelectedAnswer>()
           .HasOne(sa => sa.QuizResult)
           .WithMany(qr => qr.SelectedAnswers)
           .HasForeignKey(sa => sa.QuizResultId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SelectedAnswer>()
            .HasOne(sa => sa.Answer)
            .WithMany(a => a.SelectedAnswers)
            .HasForeignKey(sa => sa.AnswerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Quiz>().
            HasOne(q => q.CreatedByNavigation)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Role1 = "Admin" },
            new Role { Id = 2, Role1 = "User" }
        );

        modelBuilder.Entity<User>().HasData(//AnLT Đổi Tí Mật Khẩu, Mật Khẩu của chúng ta sẽ là mã hóa Bcrypt
            new User { Id = 1, RoleId = 1, Email = "admin@email.com", Username = "admin", Password = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null },
            new User { Id = 2, RoleId = 2, Email = "user1@email.com", Username = "user1", Password = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null },
            new User { Id = 3, RoleId = 2, Email = "user2@email.com", Username = "user2", Password = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null }
        );

        modelBuilder.Entity<QuestionType>().HasData(
            new QuestionType { Id = 1, Name = "Single Choice" },
            new QuestionType { Id = 2, Name = "Multiple Choice" }
        );

        modelBuilder.Entity<Quiz>().HasData(
            new Quiz { Id = 1, Name = "General Knowledge Quiz", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
            new Quiz { Id = 2, Name = "Science Trivia", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
            new Quiz { Id = 3, Name = "History Challenge", CreatedBy = 3, CreatedOn = DateTime.Now, IsActive = true }
        );

        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 1, Content = "What is the capital of France?", QuizId = 1, QuestionTypeId = 1 },
            new Question { Id = 2, Content = "Which planets are part of the solar system?", QuizId = 2, QuestionTypeId = 2 },
            new Question { Id = 3, Content = "Who was the first President of the United States?", QuizId = 3, QuestionTypeId = 1 }
        );

        modelBuilder.Entity<Answer>().HasData(
            new Answer { Id = 1, Content = "Paris", QuestionId = 1, IsCorrect = true },
            new Answer { Id = 2, Content = "London", QuestionId = 1, IsCorrect = false },
            new Answer { Id = 3, Content = "Mars", QuestionId = 2, IsCorrect = true },
            new Answer { Id = 4, Content = "Venus", QuestionId = 2, IsCorrect = true },
            new Answer { Id = 5, Content = "Pluto", QuestionId = 2, IsCorrect = false },
            new Answer { Id = 6, Content = "George Washington", QuestionId = 3, IsCorrect = true },
            new Answer { Id = 7, Content = "Thomas Jefferson", QuestionId = 3, IsCorrect = false }
        );
    }
}
