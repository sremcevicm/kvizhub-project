using Microsoft.EntityFrameworkCore;
using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasOne(q => q.Category)
                      .WithMany(c => c.Quizzes)
                      .HasForeignKey(q => q.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.Quiz)
                      .WithMany(qz => qz.Questions)
                      .HasForeignKey(q => q.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasOne(a => a.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programiranje", Description = "Kvizovi iz programiranja i informatike" },
                new Category { Id = 2, Name = "Istorija", Description = "Kvizovi iz istorije" },
                new Category { Id = 3, Name = "Nauka", Description = "Kvizovi iz prirodnih nauka" },
                new Category { Id = 4, Name = "Opšte znanje", Description = "Kvizovi opšteg znanja" }
            );
        }
    }
}
