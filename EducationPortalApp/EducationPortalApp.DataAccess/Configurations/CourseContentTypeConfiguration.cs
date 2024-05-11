using EducationPortalApp.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class CourseContentTypeConfiguration : IEntityTypeConfiguration<CourseContentType>
    {
        public void Configure(EntityTypeBuilder<CourseContentType> builder)
        {
            builder.HasData(new CourseContentType[]
            {
                new()
                {
                    Definition = "Video",
                    Id = 1,
                },
                new()
                {
                    Definition = "Book",
                    Id = 2,
                },
            });
            builder.Property(x => x.Definition).IsRequired().HasMaxLength(100);
        }
    }
}
