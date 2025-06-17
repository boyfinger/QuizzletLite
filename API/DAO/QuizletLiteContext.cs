using API.Models;
using API.Models.Enums;
using API.Models.Snapshots;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.DAO;

public partial class QuizletLiteContext : DbContext
{
    public QuizletLiteContext()
    {
    }

    public QuizletLiteContext(DbContextOptions<QuizletLiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuizAttempt>()
            .HasOne(qa => qa.User)
            .WithMany(u => u.QuizAttempts)
            .HasForeignKey(qa => qa.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.CreatedByNavigation)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>().HasData(//AnLT Đổi Tí Mật Khẩu, Mật Khẩu của chúng ta sẽ là mã hóa Bcrypt, cứ test là 123
            new User { Id = 1, Role = Role.Admin, Email = "admin@email.com", Username = "admin", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null },
            new User { Id = 2, Role = Role.User, Email = "user1@email.com", Username = "user1", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null },
            new User { Id = 3, Role = Role.User, Email = "user2@email.com", Username = "user2", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = null }
        );

        modelBuilder.Entity<Quiz>().HasData(
            new Quiz { Id = 1, Name = "General Knowledge Quiz", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
            new Quiz { Id = 2, Name = "Science Trivia", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
            new Quiz { Id = 3, Name = "History Challenge", CreatedBy = 3, CreatedOn = DateTime.Now, IsActive = true }
        );

        var question1Answers = new List<QuestionOptionsSnapshot>
        {
            new QuestionOptionsSnapshot { Content = "Paris", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "London", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Berlin", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Madrid", IsCorrect = false }
        };
        var question2Answers = new List<QuestionOptionsSnapshot>
        {
            new QuestionOptionsSnapshot { Content = "Mercury", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Venus", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Earth", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Pluto", IsCorrect = false }
        };
        var question3Answers = new List<QuestionOptionsSnapshot>
        {
            new QuestionOptionsSnapshot { Content = "George Washington", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Thomas Jefferson", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Abraham Lincoln", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "John Adams", IsCorrect = false }
        };
        var question4Answers = new List<QuestionOptionsSnapshot>
        {
            new QuestionOptionsSnapshot { Content = "Hanoi", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Ho Chi Minh City", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Da Nang", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Hue", IsCorrect = false }
        };
        var question5Answers = new List<QuestionOptionsSnapshot>
        {
            new QuestionOptionsSnapshot { Content = "Jupiter", IsCorrect = true },
            new QuestionOptionsSnapshot { Content = "Saturn", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Earth", IsCorrect = false },
            new QuestionOptionsSnapshot { Content = "Mars", IsCorrect = false }
        };

        modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 1,
                Content = "What is the capital of France?",
                QuizId = 1,
                QuestionType = QuestionType.SingleChoice,
                OptionsJson = JsonConvert.SerializeObject(question1Answers)
            },
            new Question
            {
                Id = 2,
                Content = "Which planets are part of the solar system?",
                QuizId = 2,
                QuestionType = QuestionType.MultipleChoice,
                OptionsJson = JsonConvert.SerializeObject(question2Answers)
            },
            new Question
            {
                Id = 3,
                Content = "Who was the first President of the United States?",
                QuizId = 3,
                QuestionType = QuestionType.SingleChoice,
                OptionsJson = JsonConvert.SerializeObject(question3Answers)
            },
            new Question
            {
                Id = 4,
                Content = "What is the capital of Vietnam?",
                QuizId = 1,
                QuestionType = QuestionType.SingleChoice,
                OptionsJson = JsonConvert.SerializeObject(question4Answers)
            },
            new Question
            {
                Id = 5,
                Content = "What is the largest planet in our solar system?",
                QuizId = 2,
                QuestionType = QuestionType.SingleChoice,
                OptionsJson = JsonConvert.SerializeObject(question5Answers)
            });
    }
}
