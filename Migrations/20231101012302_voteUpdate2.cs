using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class voteUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteModel",
                schema: "Identity",
                table: "VoteModel");

            migrationBuilder.RenameTable(
                name: "VoteModel",
                schema: "Identity",
                newName: "VoteModels",
                newSchema: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteModels",
                schema: "Identity",
                table: "VoteModels",
                column: "idVoteModel");

            migrationBuilder.CreateTable(
                name: "FavoritesModel",
                schema: "Identity",
                columns: table => new
                {
                    idPostModel = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostDate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesModel", x => x.idPostModel);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritesModel",
                schema: "Identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteModels",
                schema: "Identity",
                table: "VoteModels");

            migrationBuilder.RenameTable(
                name: "VoteModels",
                schema: "Identity",
                newName: "VoteModel",
                newSchema: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteModel",
                schema: "Identity",
                table: "VoteModel",
                column: "idVoteModel");
        }
    }
}
