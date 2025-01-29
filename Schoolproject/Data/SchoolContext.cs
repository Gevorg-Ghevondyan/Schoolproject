using Microsoft.EntityFrameworkCore;

namespace Schoolproject.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourFallbackConnectionStringHere");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Subjects)
                .WithMany(s => s.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassSubjects",
                    j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"),
                    j => j.HasOne<Class>().WithMany().HasForeignKey("ClassId"));

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Teachers)
                .WithMany(t => t.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassTeachers",
                    j => j.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId"),
                    j => j.HasOne<Class>().WithMany().HasForeignKey("ClassId"));

            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassStudents",
                    j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                    j => j.HasOne<Class>().WithMany().HasForeignKey("ClassId"));
        }

    }
}
