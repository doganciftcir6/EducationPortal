using EducationPortalApp.DataAccess.Configurations;
using EducationPortalApp.Entities;
using EducationPortalApp.Entities.CourseEntities;
using EducationPortalApp.Entities.EnrollmentEntities;
using EducationPortalApp.Entities.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationPortalApp.DataAccess.Contexts.EntityFramework
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {    
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<CourseContentType> CourseContentTypes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentRequest> EnrollmentRequests { get; set; }
        public DbSet<Gender> Genders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GenderConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new CourseContentConfiguration());
            builder.ApplyConfiguration(new CourseContentTypeConfiguration());
            builder.ApplyConfiguration(new EnrollmentConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
