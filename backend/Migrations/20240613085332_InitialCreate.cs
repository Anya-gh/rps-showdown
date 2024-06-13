using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerChoice = table.Column<string>(type: "TEXT", nullable: false),
                    AIChoice = table.Column<string>(type: "TEXT", nullable: false),
                    Result = table.Column<string>(type: "TEXT", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Records_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WinsBeginner = table.Column<float>(type: "REAL", nullable: false),
                    DrawsBeginner = table.Column<float>(type: "REAL", nullable: false),
                    LossesBeginner = table.Column<float>(type: "REAL", nullable: false),
                    WinsIntermediate = table.Column<float>(type: "REAL", nullable: false),
                    DrawsIntermediate = table.Column<float>(type: "REAL", nullable: false),
                    LossesIntermediate = table.Column<float>(type: "REAL", nullable: false),
                    WinsAdvanced = table.Column<float>(type: "REAL", nullable: false),
                    DrawsAdvanced = table.Column<float>(type: "REAL", nullable: false),
                    LossesAdvanced = table.Column<float>(type: "REAL", nullable: false),
                    AceChoice = table.Column<string>(type: "TEXT", nullable: false),
                    NemesisChoice = table.Column<string>(type: "TEXT", nullable: false),
                    LongestStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayStyle = table.Column<string>(type: "TEXT", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserStats_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserID",
                table: "Records",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserStats_UserID",
                table: "UserStats",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
