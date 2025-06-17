using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    OptionsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<double>(type: "float", nullable: false),
                    QuizName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswersJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, null, "admin@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 0, "admin" },
                    { 2, null, "user1@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 1, "user1" },
                    { 3, null, "user2@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 1, "user2" }
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 6, 17, 9, 59, 26, 905, DateTimeKind.Local).AddTicks(5506), true, "General Knowledge Quiz" },
                    { 2, 2, new DateTime(2025, 6, 17, 9, 59, 26, 905, DateTimeKind.Local).AddTicks(5520), true, "Science Trivia" },
                    { 3, 3, new DateTime(2025, 6, 17, 9, 59, 26, 905, DateTimeKind.Local).AddTicks(5522), true, "History Challenge" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Content", "OptionsJson", "QuestionType", "QuizId" },
                values: new object[,]
                {
                    { 1, "What is the capital of France?", "[{\"Content\":\"Paris\",\"IsCorrect\":true},{\"Content\":\"London\",\"IsCorrect\":false},{\"Content\":\"Berlin\",\"IsCorrect\":false},{\"Content\":\"Madrid\",\"IsCorrect\":false}]", 0, 1 },
                    { 2, "Which planets are part of the solar system?", "[{\"Content\":\"Mercury\",\"IsCorrect\":true},{\"Content\":\"Venus\",\"IsCorrect\":true},{\"Content\":\"Earth\",\"IsCorrect\":true},{\"Content\":\"Pluto\",\"IsCorrect\":false}]", 1, 2 },
                    { 3, "Who was the first President of the United States?", "[{\"Content\":\"George Washington\",\"IsCorrect\":true},{\"Content\":\"Thomas Jefferson\",\"IsCorrect\":false},{\"Content\":\"Abraham Lincoln\",\"IsCorrect\":false},{\"Content\":\"John Adams\",\"IsCorrect\":false}]", 0, 3 },
                    { 4, "What is the capital of Vietnam?", "[{\"Content\":\"Hanoi\",\"IsCorrect\":true},{\"Content\":\"Ho Chi Minh City\",\"IsCorrect\":false},{\"Content\":\"Da Nang\",\"IsCorrect\":false},{\"Content\":\"Hue\",\"IsCorrect\":false}]", 0, 1 },
                    { 5, "What is the largest planet in our solar system?", "[{\"Content\":\"Jupiter\",\"IsCorrect\":true},{\"Content\":\"Saturn\",\"IsCorrect\":false},{\"Content\":\"Earth\",\"IsCorrect\":false},{\"Content\":\"Mars\",\"IsCorrect\":false}]", 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_UserId",
                table: "QuizAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CreatedBy",
                table: "Quizzes",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
