using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationPortalApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursePictureColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Courses");
        }
    }
}
