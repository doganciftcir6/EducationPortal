using EducationPortalApp.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class CourseContentConfiguration : IEntityTypeConfiguration<CourseContent>
    {
        public void Configure(EntityTypeBuilder<CourseContent> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(x => x.Course).WithMany(x => x.CourseContents).HasForeignKey(x => x.CourseId);
            builder.HasOne(x => x.CourseContentType).WithMany(x => x.CourseContents).HasForeignKey(x => x.CourseContentTypeId);
        }
    }
}
