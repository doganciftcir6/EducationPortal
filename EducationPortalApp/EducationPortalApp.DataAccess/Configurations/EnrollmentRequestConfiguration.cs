using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class EnrollmentRequestConfiguration : IEntityTypeConfiguration<EnrollmentRequest>
    {
        public void Configure(EntityTypeBuilder<EnrollmentRequest> builder)
        {
            builder.HasOne(x => x.AppUser).WithMany(x => x.EnrollmentRequests).HasForeignKey(x => x.AppUserId);
            builder.HasOne(x => x.Course).WithMany(x => x.EnrollmentRequests).HasForeignKey(x => x.CourseId);
        }
    }
}
