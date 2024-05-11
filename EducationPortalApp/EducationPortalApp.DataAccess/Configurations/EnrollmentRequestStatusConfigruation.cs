using EducationPortalApp.Entities.EnrollmentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class EnrollmentRequestStatusConfigruation : IEntityTypeConfiguration<EnrollmentRequestStatus>
    {
        public void Configure(EntityTypeBuilder<EnrollmentRequestStatus> builder)
        {
            builder.HasData(new EnrollmentRequestStatus[]
          {
                new()
                {
                    Definition = "Participation",
                    Id = 1,
                },
                new()
                {
                    Definition = "Cancellation",
                    Id = 2,
                },
          });
            builder.Property(x => x.Definition).IsRequired().HasMaxLength(100);
        }
    }
}
