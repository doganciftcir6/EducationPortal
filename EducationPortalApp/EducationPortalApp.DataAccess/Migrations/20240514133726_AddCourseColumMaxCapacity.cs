using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationPortalApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseColumMaxCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxCapacity",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxCapacity",
                table: "Courses");
        }
    }
}
