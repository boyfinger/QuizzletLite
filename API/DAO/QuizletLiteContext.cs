using API.Helpers;
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

        DataSeeder.SeedData(modelBuilder);
    }
}
