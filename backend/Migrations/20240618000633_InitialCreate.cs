﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SessionItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SessionItems_LevelItems_LevelID",
                        column: x => x.LevelID,
                        principalTable: "LevelItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionItems_LevelItems_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "LevelItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionItems_UserItems_UserID",
                        column: x => x.UserID,
                        principalTable: "UserItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerChoice = table.Column<string>(type: "TEXT", nullable: false),
                    LevelChoice = table.Column<string>(type: "TEXT", nullable: false),
                    Result = table.Column<string>(type: "TEXT", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchItems_SessionItems_SessionID",
                        column: x => x.SessionID,
                        principalTable: "SessionItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchItems_UserItems_UserID",
                        column: x => x.UserID,
                        principalTable: "UserItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LevelItems",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { -1, "Player" },
                    { 1, "Beginner" },
                    { 2, "Intermediate" },
                    { 3, "Advanced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchItems_SessionID",
                table: "MatchItems",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchItems_UserID",
                table: "MatchItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionItems_LevelID",
                table: "SessionItems",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionItems_PlayerID",
                table: "SessionItems",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionItems_UserID",
                table: "SessionItems",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchItems");

            migrationBuilder.DropTable(
                name: "SessionItems");

            migrationBuilder.DropTable(
                name: "LevelItems");

            migrationBuilder.DropTable(
                name: "UserItems");
        }
    }
}
