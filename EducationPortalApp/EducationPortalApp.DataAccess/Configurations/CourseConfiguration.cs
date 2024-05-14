using EducationPortalApp.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortalApp.DataAccess.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Instructor).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Capacity).IsRequired();
            builder.Property(x => x.MaxCapacity).IsRequired();
            builder.Property(x => x.CostPerDay).IsRequired();
            builder.Property(x => x.DurationInHours).IsRequired();
            builder.Property(x => x.CreateDate).HasDefaultValueSql("getdate()");
            builder.HasOne(x => x.Category).WithMany(x => x.Courses).HasForeignKey(x => x.CategoryId);
        }
    }
}
