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
                    { 1, "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652", "admin@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 0, "admin" },
                    { 2, "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652", "user1@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 1, "user1" },
                    { 3, "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652", "user2@email.com", "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", 1, "user2" }
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8609), true, "General Knowledge Quiz" },
                    { 2, 2, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8633), true, "Science Trivia" },
                    { 3, 3, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8637), true, "History Challenge" },
                    { 4, 1, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8641), true, "Geography Quiz" },
                    { 5, 1, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8644), true, "Math Challenge" },
                    { 6, 3, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8648), true, "Literature Quiz" },
                    { 7, 2, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8651), true, "Art History Quiz" },
                    { 8, 1, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8655), true, "Technology Trends" },
                    { 9, 3, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8658), true, "World Cultures" },
                    { 10, 2, new DateTime(2025, 7, 1, 8, 4, 57, 444, DateTimeKind.Local).AddTicks(8661), true, "Sports Trivia" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Content", "OptionsJson", "QuestionType", "QuizId" },
                values: new object[,]
                {
                    { 1, "What is the capital of France?", "[{\"Content\":\"Paris\",\"IsCorrect\":true},{\"Content\":\"London\",\"IsCorrect\":false},{\"Content\":\"Berlin\",\"IsCorrect\":false},{\"Content\":\"Madrid\",\"IsCorrect\":false}]", 0, 1 },
                    { 2, "Which planets are known as gas giants?", "[{\"Content\":\"Jupiter\",\"IsCorrect\":true},{\"Content\":\"Saturn\",\"IsCorrect\":true},{\"Content\":\"Earth\",\"IsCorrect\":false},{\"Content\":\"Venus\",\"IsCorrect\":false}]", 1, 1 },
                    { 3, "Who wrote 'Romeo and Juliet'?", "[{\"Content\":\"William Shakespeare\",\"IsCorrect\":true},{\"Content\":\"Charles Dickens\",\"IsCorrect\":false},{\"Content\":\"Jane Austen\",\"IsCorrect\":false},{\"Content\":\"Mark Twain\",\"IsCorrect\":false}]", 0, 1 },
                    { 4, "Which of the following are primary colors?", "[{\"Content\":\"Red\",\"IsCorrect\":true},{\"Content\":\"Blue\",\"IsCorrect\":true},{\"Content\":\"Green\",\"IsCorrect\":false},{\"Content\":\"Yellow\",\"IsCorrect\":true}]", 1, 1 },
                    { 5, "Which ocean is the largest?", "[{\"Content\":\"Pacific Ocean\",\"IsCorrect\":true},{\"Content\":\"Atlantic Ocean\",\"IsCorrect\":false},{\"Content\":\"Indian Ocean\",\"IsCorrect\":false},{\"Content\":\"Arctic Ocean\",\"IsCorrect\":false}]", 0, 1 },
                    { 6, "Which of these animals are mammals?", "[{\"Content\":\"Whale\",\"IsCorrect\":true},{\"Content\":\"Shark\",\"IsCorrect\":false},{\"Content\":\"Bat\",\"IsCorrect\":true},{\"Content\":\"Frog\",\"IsCorrect\":false}]", 1, 1 },
                    { 7, "What is the smallest prime number?", "[{\"Content\":\"2\",\"IsCorrect\":true},{\"Content\":\"1\",\"IsCorrect\":false},{\"Content\":\"0\",\"IsCorrect\":false},{\"Content\":\"3\",\"IsCorrect\":false}]", 0, 1 },
                    { 8, "Select all countries located in South America.", "[{\"Content\":\"Brazil\",\"IsCorrect\":true},{\"Content\":\"Argentina\",\"IsCorrect\":true},{\"Content\":\"Mexico\",\"IsCorrect\":false},{\"Content\":\"Peru\",\"IsCorrect\":true}]", 1, 1 },
                    { 9, "What year did the Titanic sink?", "[{\"Content\":\"1912\",\"IsCorrect\":true},{\"Content\":\"1905\",\"IsCorrect\":false},{\"Content\":\"1920\",\"IsCorrect\":false},{\"Content\":\"1898\",\"IsCorrect\":false}]", 0, 1 },
                    { 10, "Which of the following are programming languages?", "[{\"Content\":\"Python\",\"IsCorrect\":true},{\"Content\":\"HTML\",\"IsCorrect\":false},{\"Content\":\"Java\",\"IsCorrect\":true},{\"Content\":\"CSS\",\"IsCorrect\":false}]", 1, 1 },
                    { 11, "What is the chemical symbol for gold?", "[{\"Content\":\"Au\",\"IsCorrect\":true},{\"Content\":\"Ag\",\"IsCorrect\":false},{\"Content\":\"Gd\",\"IsCorrect\":false},{\"Content\":\"Go\",\"IsCorrect\":false}]", 0, 2 },
                    { 12, "Which of the following are states of matter?", "[{\"Content\":\"Solid\",\"IsCorrect\":true},{\"Content\":\"Liquid\",\"IsCorrect\":true},{\"Content\":\"Plasma\",\"IsCorrect\":true},{\"Content\":\"Granular\",\"IsCorrect\":false}]", 1, 2 },
                    { 13, "What planet is known as the Red Planet?", "[{\"Content\":\"Mars\",\"IsCorrect\":true},{\"Content\":\"Venus\",\"IsCorrect\":false},{\"Content\":\"Jupiter\",\"IsCorrect\":false},{\"Content\":\"Neptune\",\"IsCorrect\":false}]", 0, 2 },
                    { 14, "Which of the following are components of the cell?", "[{\"Content\":\"Nucleus\",\"IsCorrect\":true},{\"Content\":\"Chloroplast\",\"IsCorrect\":true},{\"Content\":\"Mitochondria\",\"IsCorrect\":true},{\"Content\":\"Glucose\",\"IsCorrect\":false}]", 1, 2 },
                    { 15, "Which gas do plants primarily use for photosynthesis?", "[{\"Content\":\"Carbon Dioxide\",\"IsCorrect\":true},{\"Content\":\"Oxygen\",\"IsCorrect\":false},{\"Content\":\"Hydrogen\",\"IsCorrect\":false},{\"Content\":\"Nitrogen\",\"IsCorrect\":false}]", 0, 2 },
                    { 16, "Select the examples of renewable energy sources.", "[{\"Content\":\"Solar\",\"IsCorrect\":true},{\"Content\":\"Wind\",\"IsCorrect\":true},{\"Content\":\"Coal\",\"IsCorrect\":false},{\"Content\":\"Hydropower\",\"IsCorrect\":true}]", 1, 2 },
                    { 17, "What part of the atom has a positive charge?", "[{\"Content\":\"Proton\",\"IsCorrect\":true},{\"Content\":\"Electron\",\"IsCorrect\":false},{\"Content\":\"Neutron\",\"IsCorrect\":false},{\"Content\":\"Nucleus\",\"IsCorrect\":false}]", 0, 2 },
                    { 18, "Which of these are examples of amphibians?", "[{\"Content\":\"Frog\",\"IsCorrect\":true},{\"Content\":\"Salamander\",\"IsCorrect\":true},{\"Content\":\"Lizard\",\"IsCorrect\":false},{\"Content\":\"Toad\",\"IsCorrect\":true}]", 1, 2 },
                    { 19, "What is the boiling point of water at sea level?", "[{\"Content\":\"100°C\",\"IsCorrect\":true},{\"Content\":\"90°C\",\"IsCorrect\":false},{\"Content\":\"80°C\",\"IsCorrect\":false},{\"Content\":\"120°C\",\"IsCorrect\":false}]", 0, 2 },
                    { 20, "Select all planets that have rings.", "[{\"Content\":\"Saturn\",\"IsCorrect\":true},{\"Content\":\"Jupiter\",\"IsCorrect\":true},{\"Content\":\"Uranus\",\"IsCorrect\":true},{\"Content\":\"Mars\",\"IsCorrect\":false}]", 1, 2 },
                    { 21, "Who was the first President of the United States?", "[{\"Content\":\"George Washington\",\"IsCorrect\":true},{\"Content\":\"Thomas Jefferson\",\"IsCorrect\":false},{\"Content\":\"Abraham Lincoln\",\"IsCorrect\":false},{\"Content\":\"John Adams\",\"IsCorrect\":false}]", 0, 3 },
                    { 22, "Which ancient civilizations built pyramids?", "[{\"Content\":\"Egyptians\",\"IsCorrect\":true},{\"Content\":\"Mayans\",\"IsCorrect\":true},{\"Content\":\"Romans\",\"IsCorrect\":false},{\"Content\":\"Aztecs\",\"IsCorrect\":true}]", 1, 3 },
                    { 23, "In what year did World War II end?", "[{\"Content\":\"1945\",\"IsCorrect\":true},{\"Content\":\"1939\",\"IsCorrect\":false},{\"Content\":\"1941\",\"IsCorrect\":false},{\"Content\":\"1950\",\"IsCorrect\":false}]", 0, 3 },
                    { 24, "Which of these explorers sailed around the world?", "[{\"Content\":\"Ferdinand Magellan\",\"IsCorrect\":true},{\"Content\":\"Francis Drake\",\"IsCorrect\":true},{\"Content\":\"Marco Polo\",\"IsCorrect\":false},{\"Content\":\"James Cook\",\"IsCorrect\":false}]", 1, 3 },
                    { 25, "The Berlin Wall fell in what year?", "[{\"Content\":\"1989\",\"IsCorrect\":true},{\"Content\":\"1991\",\"IsCorrect\":false},{\"Content\":\"1985\",\"IsCorrect\":false},{\"Content\":\"1990\",\"IsCorrect\":false}]", 0, 3 },
                    { 26, "Which empires once ruled over large parts of Europe?", "[{\"Content\":\"Roman Empire\",\"IsCorrect\":true},{\"Content\":\"Ottoman Empire\",\"IsCorrect\":true},{\"Content\":\"Ming Dynasty\",\"IsCorrect\":false},{\"Content\":\"Byzantine Empire\",\"IsCorrect\":true}]", 1, 3 },
                    { 27, "Who was known as the 'Maid of Orléans'?", "[{\"Content\":\"Joan of Arc\",\"IsCorrect\":true},{\"Content\":\"Marie Antoinette\",\"IsCorrect\":false},{\"Content\":\"Catherine the Great\",\"IsCorrect\":false},{\"Content\":\"Elizabeth I\",\"IsCorrect\":false}]", 0, 3 },
                    { 28, "Which countries were part of the Axis powers in World War II?", "[{\"Content\":\"Germany\",\"IsCorrect\":true},{\"Content\":\"Italy\",\"IsCorrect\":true},{\"Content\":\"Japan\",\"IsCorrect\":true},{\"Content\":\"France\",\"IsCorrect\":false}]", 1, 3 },
                    { 29, "Who was the British Prime Minister during most of World War II?", "[{\"Content\":\"Winston Churchill\",\"IsCorrect\":true},{\"Content\":\"Neville Chamberlain\",\"IsCorrect\":false},{\"Content\":\"Clement Attlee\",\"IsCorrect\":false},{\"Content\":\"Tony Blair\",\"IsCorrect\":false}]", 0, 3 },
                    { 30, "Select events that took place during the 20th century.", "[{\"Content\":\"The Moon Landing\",\"IsCorrect\":true},{\"Content\":\"The Cold War\",\"IsCorrect\":true},{\"Content\":\"French Revolution\",\"IsCorrect\":false},{\"Content\":\"Fall of the Soviet Union\",\"IsCorrect\":true}]", 1, 3 },
                    { 31, "Which is the longest river in the world?", "[{\"Content\":\"Nile\",\"IsCorrect\":true},{\"Content\":\"Amazon\",\"IsCorrect\":false},{\"Content\":\"Yangtze\",\"IsCorrect\":false},{\"Content\":\"Mississippi\",\"IsCorrect\":false}]", 0, 4 },
                    { 32, "Which countries are landlocked?", "[{\"Content\":\"Nepal\",\"IsCorrect\":true},{\"Content\":\"Switzerland\",\"IsCorrect\":true},{\"Content\":\"Thailand\",\"IsCorrect\":false},{\"Content\":\"Austria\",\"IsCorrect\":true}]", 1, 4 },
                    { 33, "What is the smallest country in the world by area?", "[{\"Content\":\"Vatican City\",\"IsCorrect\":true},{\"Content\":\"Monaco\",\"IsCorrect\":false},{\"Content\":\"San Marino\",\"IsCorrect\":false},{\"Content\":\"Liechtenstein\",\"IsCorrect\":false}]", 0, 4 },
                    { 34, "Select the countries that span more than one continent.", "[{\"Content\":\"Russia\",\"IsCorrect\":true},{\"Content\":\"Turkey\",\"IsCorrect\":true},{\"Content\":\"Egypt\",\"IsCorrect\":true},{\"Content\":\"Canada\",\"IsCorrect\":false}]", 1, 4 },
                    { 35, "What is the capital of Canada?", "[{\"Content\":\"Ottawa\",\"IsCorrect\":true},{\"Content\":\"Toronto\",\"IsCorrect\":false},{\"Content\":\"Vancouver\",\"IsCorrect\":false},{\"Content\":\"Montreal\",\"IsCorrect\":false}]", 0, 4 },
                    { 36, "Which continents lie entirely in the Southern Hemisphere?", "[{\"Content\":\"Australia\",\"IsCorrect\":true},{\"Content\":\"Antarctica\",\"IsCorrect\":true},{\"Content\":\"Africa\",\"IsCorrect\":false},{\"Content\":\"South America\",\"IsCorrect\":false}]", 1, 4 },
                    { 37, "Mount Everest lies in which mountain range?", "[{\"Content\":\"Himalayas\",\"IsCorrect\":true},{\"Content\":\"Andes\",\"IsCorrect\":false},{\"Content\":\"Alps\",\"IsCorrect\":false},{\"Content\":\"Rockies\",\"IsCorrect\":false}]", 0, 4 },
                    { 38, "Select all countries through which the Equator passes.", "[{\"Content\":\"Ecuador\",\"IsCorrect\":true},{\"Content\":\"Kenya\",\"IsCorrect\":true},{\"Content\":\"Brazil\",\"IsCorrect\":true},{\"Content\":\"Mexico\",\"IsCorrect\":false}]", 1, 4 },
                    { 39, "What desert is the largest in the world by area?", "[{\"Content\":\"Antarctic Desert\",\"IsCorrect\":true},{\"Content\":\"Sahara\",\"IsCorrect\":false},{\"Content\":\"Gobi\",\"IsCorrect\":false},{\"Content\":\"Arctic\",\"IsCorrect\":false}]", 0, 4 },
                    { 40, "Which of these are island nations?", "[{\"Content\":\"Japan\",\"IsCorrect\":true},{\"Content\":\"Iceland\",\"IsCorrect\":true},{\"Content\":\"Norway\",\"IsCorrect\":false},{\"Content\":\"New Zealand\",\"IsCorrect\":true}]", 1, 4 },
                    { 41, "What is the value of π (pi) rounded to two decimal places?", "[{\"Content\":\"3.14\",\"IsCorrect\":true},{\"Content\":\"2.71\",\"IsCorrect\":false},{\"Content\":\"1.61\",\"IsCorrect\":false},{\"Content\":\"3.41\",\"IsCorrect\":false}]", 0, 5 },
                    { 42, "Which of the following numbers are prime?", "[{\"Content\":\"2\",\"IsCorrect\":true},{\"Content\":\"3\",\"IsCorrect\":true},{\"Content\":\"4\",\"IsCorrect\":false},{\"Content\":\"5\",\"IsCorrect\":true}]", 1, 5 },
                    { 43, "What is the square root of 144?", "[{\"Content\":\"12\",\"IsCorrect\":true},{\"Content\":\"14\",\"IsCorrect\":false},{\"Content\":\"11\",\"IsCorrect\":false},{\"Content\":\"10\",\"IsCorrect\":false}]", 0, 5 },
                    { 44, "Select all even numbers.", "[{\"Content\":\"6\",\"IsCorrect\":true},{\"Content\":\"11\",\"IsCorrect\":false},{\"Content\":\"14\",\"IsCorrect\":true},{\"Content\":\"9\",\"IsCorrect\":false}]", 1, 5 },
                    { 45, "What is 7 × 8?", "[{\"Content\":\"56\",\"IsCorrect\":true},{\"Content\":\"64\",\"IsCorrect\":false},{\"Content\":\"48\",\"IsCorrect\":false},{\"Content\":\"52\",\"IsCorrect\":false}]", 0, 5 },
                    { 46, "Which of these are multiples of 3?", "[{\"Content\":\"9\",\"IsCorrect\":true},{\"Content\":\"12\",\"IsCorrect\":true},{\"Content\":\"14\",\"IsCorrect\":false},{\"Content\":\"15\",\"IsCorrect\":true}]", 1, 5 },
                    { 47, "What is the value of 3²?", "[{\"Content\":\"9\",\"IsCorrect\":true},{\"Content\":\"6\",\"IsCorrect\":false},{\"Content\":\"8\",\"IsCorrect\":false},{\"Content\":\"12\",\"IsCorrect\":false}]", 0, 5 },
                    { 48, "Select all factors of 24.", "[{\"Content\":\"2\",\"IsCorrect\":true},{\"Content\":\"3\",\"IsCorrect\":true},{\"Content\":\"5\",\"IsCorrect\":false},{\"Content\":\"8\",\"IsCorrect\":true}]", 1, 5 },
                    { 49, "Which number is both a square and a cube?", "[{\"Content\":\"64\",\"IsCorrect\":true},{\"Content\":\"16\",\"IsCorrect\":false},{\"Content\":\"81\",\"IsCorrect\":false},{\"Content\":\"100\",\"IsCorrect\":false}]", 0, 5 },
                    { 50, "Which of the following equations are true?", "[{\"Content\":\"5 + 3 = 8\",\"IsCorrect\":true},{\"Content\":\"10 ÷ 2 = 4\",\"IsCorrect\":false},{\"Content\":\"6 × 6 = 36\",\"IsCorrect\":true},{\"Content\":\"7 - 2 = 6\",\"IsCorrect\":false}]", 1, 5 },
                    { 51, "Who wrote '1984'?", "[{\"Content\":\"George Orwell\",\"IsCorrect\":true},{\"Content\":\"Aldous Huxley\",\"IsCorrect\":false},{\"Content\":\"Ray Bradbury\",\"IsCorrect\":false},{\"Content\":\"Ernest Hemingway\",\"IsCorrect\":false}]", 0, 6 },
                    { 52, "Which of the following are works by William Shakespeare?", "[{\"Content\":\"Hamlet\",\"IsCorrect\":true},{\"Content\":\"Macbeth\",\"IsCorrect\":true},{\"Content\":\"Frankenstein\",\"IsCorrect\":false},{\"Content\":\"Othello\",\"IsCorrect\":true}]", 1, 6 },
                    { 53, "Who is the author of 'Pride and Prejudice'?", "[{\"Content\":\"Jane Austen\",\"IsCorrect\":true},{\"Content\":\"Charlotte Brontë\",\"IsCorrect\":false},{\"Content\":\"Emily Dickinson\",\"IsCorrect\":false},{\"Content\":\"Mary Shelley\",\"IsCorrect\":false}]", 0, 6 },
                    { 54, "Select all Greek epic poems.", "[{\"Content\":\"The Iliad\",\"IsCorrect\":true},{\"Content\":\"The Odyssey\",\"IsCorrect\":true},{\"Content\":\"Beowulf\",\"IsCorrect\":false},{\"Content\":\"The Aeneid\",\"IsCorrect\":false}]", 1, 6 },
                    { 55, "What is the real name of Mark Twain?", "[{\"Content\":\"Samuel Clemens\",\"IsCorrect\":true},{\"Content\":\"Theodore Dreiser\",\"IsCorrect\":false},{\"Content\":\"F. Scott Fitzgerald\",\"IsCorrect\":false},{\"Content\":\"Henry James\",\"IsCorrect\":false}]", 0, 6 },
                    { 56, "Which characters appear in 'The Great Gatsby'?", "[{\"Content\":\"Jay Gatsby\",\"IsCorrect\":true},{\"Content\":\"Daisy Buchanan\",\"IsCorrect\":true},{\"Content\":\"Tom Buchanan\",\"IsCorrect\":true},{\"Content\":\"Holden Caulfield\",\"IsCorrect\":false}]", 1, 6 },
                    { 57, "Who wrote the poem 'The Raven'?", "[{\"Content\":\"Edgar Allan Poe\",\"IsCorrect\":true},{\"Content\":\"Robert Frost\",\"IsCorrect\":false},{\"Content\":\"Walt Whitman\",\"IsCorrect\":false},{\"Content\":\"Emily Dickinson\",\"IsCorrect\":false}]", 0, 6 },
                    { 58, "Which of the following are literary genres?", "[{\"Content\":\"Tragedy\",\"IsCorrect\":true},{\"Content\":\"Satire\",\"IsCorrect\":true},{\"Content\":\"Suspense\",\"IsCorrect\":true},{\"Content\":\"Cartography\",\"IsCorrect\":false}]", 1, 6 },
                    { 59, "Who wrote 'To Kill a Mockingbird'?", "[{\"Content\":\"Harper Lee\",\"IsCorrect\":true},{\"Content\":\"Toni Morrison\",\"IsCorrect\":false},{\"Content\":\"J.D. Salinger\",\"IsCorrect\":false},{\"Content\":\"John Steinbeck\",\"IsCorrect\":false}]", 0, 6 },
                    { 60, "Select all novels set in dystopian futures.", "[{\"Content\":\"1984\",\"IsCorrect\":true},{\"Content\":\"Brave New World\",\"IsCorrect\":true},{\"Content\":\"The Handmaid's Tale\",\"IsCorrect\":true},{\"Content\":\"The Catcher in the Rye\",\"IsCorrect\":false}]", 1, 6 },
                    { 61, "Who painted the Mona Lisa?", "[{\"Content\":\"Leonardo da Vinci\",\"IsCorrect\":true},{\"Content\":\"Michelangelo\",\"IsCorrect\":false},{\"Content\":\"Raphael\",\"IsCorrect\":false},{\"Content\":\"Vincent van Gogh\",\"IsCorrect\":false}]", 0, 7 },
                    { 62, "Which of the following are characteristics of Baroque art?", "[{\"Content\":\"Dramatic lighting\",\"IsCorrect\":true},{\"Content\":\"Emotional intensity\",\"IsCorrect\":true},{\"Content\":\"Flat perspective\",\"IsCorrect\":false},{\"Content\":\"Rich, deep color\",\"IsCorrect\":true}]", 1, 7 },
                    { 63, "In which country did Impressionism originate?", "[{\"Content\":\"France\",\"IsCorrect\":true},{\"Content\":\"Italy\",\"IsCorrect\":false},{\"Content\":\"Germany\",\"IsCorrect\":false},{\"Content\":\"Spain\",\"IsCorrect\":false}]", 0, 7 },
                    { 64, "Which artists were part of the Cubist movement?", "[{\"Content\":\"Pablo Picasso\",\"IsCorrect\":true},{\"Content\":\"Georges Braque\",\"IsCorrect\":true},{\"Content\":\"Claude Monet\",\"IsCorrect\":false},{\"Content\":\"Henri Matisse\",\"IsCorrect\":false}]", 1, 7 },
                    { 65, "What is the name of Michelangelo's famous sculpture housed in the Vatican?", "[{\"Content\":\"Pietà\",\"IsCorrect\":true},{\"Content\":\"David\",\"IsCorrect\":false},{\"Content\":\"Laocoön and His Sons\",\"IsCorrect\":false},{\"Content\":\"The Thinker\",\"IsCorrect\":false}]", 0, 7 },
                    { 66, "Which of the following are famous works of Surrealism?", "[{\"Content\":\"The Persistence of Memory\",\"IsCorrect\":true},{\"Content\":\"The Elephants\",\"IsCorrect\":true},{\"Content\":\"Water Lilies\",\"IsCorrect\":false},{\"Content\":\"The Treachery of Images\",\"IsCorrect\":true}]", 1, 7 },
                    { 67, "Who painted 'The Starry Night'?", "[{\"Content\":\"Vincent van Gogh\",\"IsCorrect\":true},{\"Content\":\"Edvard Munch\",\"IsCorrect\":false},{\"Content\":\"Paul Cézanne\",\"IsCorrect\":false},{\"Content\":\"Henri Rousseau\",\"IsCorrect\":false}]", 0, 7 },
                    { 68, "Select all artworks that are housed in the Louvre Museum.", "[{\"Content\":\"Mona Lisa\",\"IsCorrect\":true},{\"Content\":\"Liberty Leading the People\",\"IsCorrect\":true},{\"Content\":\"The Birth of Venus\",\"IsCorrect\":false},{\"Content\":\"The Raft of the Medusa\",\"IsCorrect\":true}]", 1, 7 },
                    { 69, "Which artist is known for cutting off his own ear?", "[{\"Content\":\"Vincent van Gogh\",\"IsCorrect\":true},{\"Content\":\"Paul Gauguin\",\"IsCorrect\":false},{\"Content\":\"Édouard Manet\",\"IsCorrect\":false},{\"Content\":\"Gustav Klimt\",\"IsCorrect\":false}]", 0, 7 },
                    { 70, "Which of the following are considered Renaissance artists?", "[{\"Content\":\"Leonardo da Vinci\",\"IsCorrect\":true},{\"Content\":\"Michelangelo\",\"IsCorrect\":true},{\"Content\":\"Sandro Botticelli\",\"IsCorrect\":true},{\"Content\":\"Salvador Dalí\",\"IsCorrect\":false}]", 1, 7 },
                    { 71, "Which technology is widely used in virtual assistants like Alexa and Siri?", "[{\"Content\":\"Natural Language Processing\",\"IsCorrect\":true},{\"Content\":\"Blockchain\",\"IsCorrect\":false},{\"Content\":\"Quantum Computing\",\"IsCorrect\":false},{\"Content\":\"3D Printing\",\"IsCorrect\":false}]", 0, 8 },
                    { 72, "Which of these are considered part of Industry 4.0?", "[{\"Content\":\"Internet of Things (IoT)\",\"IsCorrect\":true},{\"Content\":\"Artificial Intelligence\",\"IsCorrect\":true},{\"Content\":\"Assembly Lines (Industry 2.0)\",\"IsCorrect\":false},{\"Content\":\"Cloud Computing\",\"IsCorrect\":true}]", 1, 8 },
                    { 73, "What is the primary purpose of blockchain technology?", "[{\"Content\":\"Decentralized record-keeping\",\"IsCorrect\":true},{\"Content\":\"Faster internet speeds\",\"IsCorrect\":false},{\"Content\":\"Quantum encryption\",\"IsCorrect\":false},{\"Content\":\"Real-time image rendering\",\"IsCorrect\":false}]", 0, 8 },
                    { 74, "Which technologies are used in renewable energy systems?", "[{\"Content\":\"Solar Panels\",\"IsCorrect\":true},{\"Content\":\"Wind Turbines\",\"IsCorrect\":true},{\"Content\":\"Diesel Generators\",\"IsCorrect\":false},{\"Content\":\"Hydroelectric Dams\",\"IsCorrect\":true}]", 1, 8 },
                    { 75, "What does GPT in ChatGPT stand for?", "[{\"Content\":\"Generative Pre-trained Transformer\",\"IsCorrect\":true},{\"Content\":\"General Purpose Technology\",\"IsCorrect\":false},{\"Content\":\"Global Processing Tool\",\"IsCorrect\":false},{\"Content\":\"Graphic Performance Tracker\",\"IsCorrect\":false}]", 0, 8 },
                    { 76, "Which of these are applications of generative AI?", "[{\"Content\":\"Image generation\",\"IsCorrect\":true},{\"Content\":\"Music composition\",\"IsCorrect\":true},{\"Content\":\"Social media moderation\",\"IsCorrect\":false},{\"Content\":\"Text summarization\",\"IsCorrect\":true}]", 1, 8 },
                    { 77, "Which company developed the first iPhone?", "[{\"Content\":\"Apple\",\"IsCorrect\":true},{\"Content\":\"Google\",\"IsCorrect\":false},{\"Content\":\"Microsoft\",\"IsCorrect\":false},{\"Content\":\"Samsung\",\"IsCorrect\":false}]", 0, 8 },
                    { 78, "Which of the following are wearable technologies?", "[{\"Content\":\"Smartwatches\",\"IsCorrect\":true},{\"Content\":\"Fitness Trackers\",\"IsCorrect\":true},{\"Content\":\"Laptops\",\"IsCorrect\":false},{\"Content\":\"AR Glasses\",\"IsCorrect\":true}]", 1, 8 },
                    { 79, "What does 5G primarily improve over 4G?", "[{\"Content\":\"Network speed and latency\",\"IsCorrect\":true},{\"Content\":\"Display resolution\",\"IsCorrect\":false},{\"Content\":\"Battery life\",\"IsCorrect\":false},{\"Content\":\"Color accuracy\",\"IsCorrect\":false}]", 0, 8 },
                    { 80, "Which of these are examples of edge computing devices?", "[{\"Content\":\"Smart thermostats\",\"IsCorrect\":true},{\"Content\":\"Self-driving cars\",\"IsCorrect\":true},{\"Content\":\"Cloud data centers\",\"IsCorrect\":false},{\"Content\":\"Drones with onboard AI\",\"IsCorrect\":true}]", 1, 8 },
                    { 81, "Which country is famous for the tea ceremony known as Chanoyu?", "[{\"Content\":\"Japan\",\"IsCorrect\":true},{\"Content\":\"China\",\"IsCorrect\":false},{\"Content\":\"India\",\"IsCorrect\":false},{\"Content\":\"Thailand\",\"IsCorrect\":false}]", 0, 9 },
                    { 82, "Which of the following are traditional forms of dance?", "[{\"Content\":\"Flamenco\",\"IsCorrect\":true},{\"Content\":\"Ballet\",\"IsCorrect\":true},{\"Content\":\"Samba\",\"IsCorrect\":true},{\"Content\":\"Karate\",\"IsCorrect\":false}]", 1, 9 },
                    { 83, "Diwali is a major festival celebrated in which country?", "[{\"Content\":\"India\",\"IsCorrect\":true},{\"Content\":\"Pakistan\",\"IsCorrect\":false},{\"Content\":\"Nepal\",\"IsCorrect\":false},{\"Content\":\"Sri Lanka\",\"IsCorrect\":false}]", 0, 9 },
                    { 84, "Which of the following are official languages of the United Nations?", "[{\"Content\":\"English\",\"IsCorrect\":true},{\"Content\":\"Arabic\",\"IsCorrect\":true},{\"Content\":\"Portuguese\",\"IsCorrect\":false},{\"Content\":\"Russian\",\"IsCorrect\":true}]", 1, 9 },
                    { 85, "Which country is known for the cultural tradition of wearing kilts?", "[{\"Content\":\"Scotland\",\"IsCorrect\":true},{\"Content\":\"Ireland\",\"IsCorrect\":false},{\"Content\":\"Wales\",\"IsCorrect\":false},{\"Content\":\"Norway\",\"IsCorrect\":false}]", 0, 9 },
                    { 86, "Which of the following are traditional cuisines?", "[{\"Content\":\"Sushi (Japan)\",\"IsCorrect\":true},{\"Content\":\"Tacos (Mexico)\",\"IsCorrect\":true},{\"Content\":\"Pizza (Italy)\",\"IsCorrect\":true},{\"Content\":\"Poutine (Australia)\",\"IsCorrect\":false}]", 1, 9 },
                    { 87, "Carnival is a major cultural event famously held in which Brazilian city?", "[{\"Content\":\"Rio de Janeiro\",\"IsCorrect\":true},{\"Content\":\"São Paulo\",\"IsCorrect\":false},{\"Content\":\"Brasília\",\"IsCorrect\":false},{\"Content\":\"Salvador\",\"IsCorrect\":false}]", 0, 9 },
                    { 88, "Which of the following are elements of traditional Chinese culture?", "[{\"Content\":\"Calligraphy\",\"IsCorrect\":true},{\"Content\":\"Tai Chi\",\"IsCorrect\":true},{\"Content\":\"Manga\",\"IsCorrect\":false},{\"Content\":\"Dragon Dance\",\"IsCorrect\":true}]", 1, 9 },
                    { 89, "What is the primary religion practiced in Thailand?", "[{\"Content\":\"Buddhism\",\"IsCorrect\":true},{\"Content\":\"Islam\",\"IsCorrect\":false},{\"Content\":\"Christianity\",\"IsCorrect\":false},{\"Content\":\"Hinduism\",\"IsCorrect\":false}]", 0, 9 },
                    { 90, "Select festivals that celebrate the lunar calendar.", "[{\"Content\":\"Lunar New Year\",\"IsCorrect\":true},{\"Content\":\"Mid-Autumn Festival\",\"IsCorrect\":true},{\"Content\":\"Thanksgiving\",\"IsCorrect\":false},{\"Content\":\"Eid al-Fitr\",\"IsCorrect\":true}]", 1, 9 },
                    { 91, "Which country has won the most FIFA World Cup titles?", "[{\"Content\":\"Brazil\",\"IsCorrect\":true},{\"Content\":\"Germany\",\"IsCorrect\":false},{\"Content\":\"Argentina\",\"IsCorrect\":false},{\"Content\":\"Italy\",\"IsCorrect\":false}]", 0, 10 },
                    { 92, "Which of the following are Olympic events?", "[{\"Content\":\"Swimming\",\"IsCorrect\":true},{\"Content\":\"Gymnastics\",\"IsCorrect\":true},{\"Content\":\"Cricket\",\"IsCorrect\":false},{\"Content\":\"Track and Field\",\"IsCorrect\":true}]", 1, 10 },
                    { 93, "In which sport is the term 'love' used to indicate a score of zero?", "[{\"Content\":\"Tennis\",\"IsCorrect\":true},{\"Content\":\"Badminton\",\"IsCorrect\":false},{\"Content\":\"Squash\",\"IsCorrect\":false},{\"Content\":\"Table Tennis\",\"IsCorrect\":false}]", 0, 10 },
                    { 94, "Which of these are considered major golf championships?", "[{\"Content\":\"The Masters\",\"IsCorrect\":true},{\"Content\":\"US Open\",\"IsCorrect\":true},{\"Content\":\"Wimbledon\",\"IsCorrect\":false},{\"Content\":\"The Open Championship\",\"IsCorrect\":true}]", 1, 10 },
                    { 95, "Which country hosted the 2020 Summer Olympics (held in 2021)?", "[{\"Content\":\"Japan\",\"IsCorrect\":true},{\"Content\":\"China\",\"IsCorrect\":false},{\"Content\":\"Brazil\",\"IsCorrect\":false},{\"Content\":\"United Kingdom\",\"IsCorrect\":false}]", 0, 10 },
                    { 96, "Which of these sports involve a ball?", "[{\"Content\":\"Basketball\",\"IsCorrect\":true},{\"Content\":\"Volleyball\",\"IsCorrect\":true},{\"Content\":\"Judo\",\"IsCorrect\":false},{\"Content\":\"Cricket\",\"IsCorrect\":true}]", 1, 10 },
                    { 97, "Which famous boxer was known as 'The Greatest'?", "[{\"Content\":\"Muhammad Ali\",\"IsCorrect\":true},{\"Content\":\"Mike Tyson\",\"IsCorrect\":false},{\"Content\":\"Floyd Mayweather\",\"IsCorrect\":false},{\"Content\":\"Joe Frazier\",\"IsCorrect\":false}]", 0, 10 },
                    { 98, "Which of the following are winter sports?", "[{\"Content\":\"Skiing\",\"IsCorrect\":true},{\"Content\":\"Snowboarding\",\"IsCorrect\":true},{\"Content\":\"Surfing\",\"IsCorrect\":false},{\"Content\":\"Figure Skating\",\"IsCorrect\":true}]", 1, 10 },
                    { 99, "How many players are on a soccer team on the field (per side)?", "[{\"Content\":\"11\",\"IsCorrect\":true},{\"Content\":\"10\",\"IsCorrect\":false},{\"Content\":\"12\",\"IsCorrect\":false},{\"Content\":\"9\",\"IsCorrect\":false}]", 0, 10 },
                    { 100, "Which of the following sports originated in the USA?", "[{\"Content\":\"Basketball\",\"IsCorrect\":true},{\"Content\":\"Baseball\",\"IsCorrect\":true},{\"Content\":\"Rugby\",\"IsCorrect\":false},{\"Content\":\"American Football\",\"IsCorrect\":true}]", 1, 10 }
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
