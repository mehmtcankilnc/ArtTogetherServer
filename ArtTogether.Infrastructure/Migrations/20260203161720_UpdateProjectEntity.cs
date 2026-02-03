using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtTogether.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Projects");
        }
    }
}
