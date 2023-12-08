using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class voteUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteModels",
                schema: "Identity",
                table: "VoteModels");

            migrationBuilder.RenameTable(
                name: "VoteModels",
                schema: "Identity",
                newName: "VoteModel",
                newSchema: "Identity");

            migrationBuilder.AlterColumn<int>(
                name: "NumPromotions",
                schema: "Identity",
                table: "PostModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumDemotions",
                schema: "Identity",
                table: "PostModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumFlags",
                schema: "Identity",
                table: "PostModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteModel",
                schema: "Identity",
                table: "VoteModel",
                column: "idVoteModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteModel",
                schema: "Identity",
                table: "VoteModel");

            migrationBuilder.DropColumn(
                name: "NumDemotions",
                schema: "Identity",
                table: "PostModel");

            migrationBuilder.DropColumn(
                name: "NumFlags",
                schema: "Identity",
                table: "PostModel");

            migrationBuilder.RenameTable(
                name: "VoteModel",
                schema: "Identity",
                newName: "VoteModels",
                newSchema: "Identity");

            migrationBuilder.AlterColumn<bool>(
                name: "NumPromotions",
                schema: "Identity",
                table: "PostModel",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteModels",
                schema: "Identity",
                table: "VoteModels",
                column: "idVoteModel");
        }
    }
}
