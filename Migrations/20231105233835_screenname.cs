using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class screenname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScreenName",
                schema: "Identity",
                table: "PostModel",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScreenName",
                schema: "Identity",
                table: "PostModel");
        }
    }
}
