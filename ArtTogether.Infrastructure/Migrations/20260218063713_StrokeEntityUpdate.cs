using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtTogether.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StrokeEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Strokes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Strokes");
        }
    }
}
