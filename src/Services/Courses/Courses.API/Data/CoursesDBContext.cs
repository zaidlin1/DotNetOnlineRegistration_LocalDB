using Courses.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.API.Models
{
    public partial class CoursesDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CoursesDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public CoursesDBContext(DbContextOptions<CoursesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<MyLesson> MyLessons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings:CoursesDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TeacherName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyLesson>(entity =>
            {
                entity.Property(e => e.Day)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Time)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.MyLessons)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Lessons_ToTable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
