using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasOne(x => x.AppUser).WithMany(x => x.Enrollments).HasForeignKey(x => x.AppUserId);
            builder.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId);
        }
    }
}
