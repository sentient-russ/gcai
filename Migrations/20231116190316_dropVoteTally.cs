using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gcai.Migrations
{
    /// <inheritdoc />
    public partial class dropVoteTally : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteTallyModel",
                schema: "Identity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoteTallyModel",
                schema: "Identity",
                columns: table => new
                {
                    PostRefNum = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DownVotedTotal = table.Column<int>(type: "int", nullable: true),
                    FlaggedTotal = table.Column<int>(type: "int", nullable: true),
                    StarVotedTotal = table.Column<int>(type: "int", nullable: true),
                    UpVotedTotal = table.Column<int>(type: "int", nullable: true),
                    UserDownVoted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserFlagged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserStarVoted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserUpVoted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteTallyModel", x => x.PostRefNum);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
