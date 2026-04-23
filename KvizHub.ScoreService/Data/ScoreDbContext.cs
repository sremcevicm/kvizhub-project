using Microsoft.EntityFrameworkCore;
using KvizHub.ScoreService.Models.Entities;

namespace KvizHub.ScoreService.Data
{
    public class ScoreDbContext : DbContext
    {
        public ScoreDbContext(DbContextOptions<ScoreDbContext> options) : base(options) { }

        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<AttemptAnswer> AttemptAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuizAttempt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.QuizId);
                entity.HasMany(e => e.Answers)
                      .WithOne(a => a.QuizAttempt)
                      .HasForeignKey(a => a.QuizAttemptId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AttemptAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
