using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class DateFieldUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SobrietyDate",
                schema: "Identity",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "SobrietyDate",
                schema: "Identity",
                table: "Users",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
