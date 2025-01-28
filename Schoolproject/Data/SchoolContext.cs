using Microsoft.EntityFrameworkCore;

namespace Schoolproject.Data
{
    public class SchoolContext : DbContext
    {
        // Constructor accepting DbContextOptions
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        // DbSets for your models
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
