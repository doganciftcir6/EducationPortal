using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationPortalApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InsertUserCourseContentStatusesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseContents");

            migrationBuilder.CreateTable(
                name: "UserCourseContentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CourseContentId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourseContentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCourseContentStatuses_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourseContentStatuses_CourseContents_CourseContentId",
                        column: x => x.CourseContentId,
                        principalTable: "CourseContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCourseContentStatuses_AppUserId",
                table: "UserCourseContentStatuses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourseContentStatuses_CourseContentId",
                table: "UserCourseContentStatuses",
                column: "CourseContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCourseContentStatuses");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "CourseContents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
