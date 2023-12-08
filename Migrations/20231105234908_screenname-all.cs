using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class screennameall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScreenName",
                schema: "Identity",
                table: "VoteModels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ScreenName",
                schema: "Identity",
                table: "AIModel",
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
                table: "VoteModels");

            migrationBuilder.DropColumn(
                name: "ScreenName",
                schema: "Identity",
                table: "AIModel");
        }
    }
}
