using Microsoft.EntityFrameworkCore;
using school.Models;

namespace school.Data
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Roles> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TEST"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithOne(u => u.Class)
                .HasForeignKey(u => u.ClassId)
                .IsRequired(false);
            modelBuilder.Entity<School>().Property(s => s.Capacity).HasDefaultValue(1000);
            modelBuilder.Entity<School>().Property(s => s.SchoolName).HasMaxLength(100);
            modelBuilder.Entity<Faculty>().Property(s => s.FacultyName).HasMaxLength(50);
            modelBuilder.Entity<Class>().Property(s => s.ClassName).HasMaxLength(50);
            modelBuilder.Entity<School>().HasIndex(f => f.SchoolName).IsUnique();
            modelBuilder.Entity<Faculty>().HasIndex(f => new { f.FacultyName, f.SchoolId }).IsUnique();
            modelBuilder.Entity<Class>().HasIndex(f => new { f.ClassName, f.FacultyId }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
