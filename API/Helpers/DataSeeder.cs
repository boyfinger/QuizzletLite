using API.Models.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Models.Snapshots;

namespace API.Helpers
{
    public static class DataSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedQuizzes(modelBuilder);
            SeedQuestions(modelBuilder);
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(//AnLT Đổi Tí Mật Khẩu, Mật Khẩu của chúng ta sẽ là mã hóa Bcrypt, cứ test là 123
                new User { Id = 1, Role = Role.Admin, Email = "admin@email.com", Username = "admin", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652" },
                new User { Id = 2, Role = Role.User, Email = "user1@email.com", Username = "user1", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652" },
                new User { Id = 3, Role = Role.User, Email = "user2@email.com", Username = "user2", PasswordHash = "$2a$12$6GMyaacdt22VmPBouyUnB.e/4guoGG09ukoXkx/eb02bKcwokoy9C", Avatar = "https://api.dicebear.com/7.x/bottts/svg?seed=1750601570652" }
            );
        }

        private static void SeedQuizzes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>().HasData(
                new Quiz { Id = 1, Name = "General Knowledge Quiz", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 2, Name = "Science Trivia", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 3, Name = "History Challenge", CreatedBy = 3, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 4, Name = "Geography Quiz", CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 5, Name = "Math Challenge", CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 6, Name = "Literature Quiz", CreatedBy = 3, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 7, Name = "Art History Quiz", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 8, Name = "Technology Trends", CreatedBy = 1, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 9, Name = "World Cultures", CreatedBy = 3, CreatedOn = DateTime.Now, IsActive = true },
                new Quiz { Id = 10, Name = "Sports Trivia", CreatedBy = 2, CreatedOn = DateTime.Now, IsActive = true }
            );
        }

        private static void SeedQuestions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    QuizId = 1,
                    Content = "What is the capital of France?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Paris", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "London", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Berlin", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Madrid", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 2,
                    QuizId = 1,
                    Content = "Which planets are known as gas giants?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Jupiter", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Saturn", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Earth", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Venus", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 3,
                    QuizId = 1,
                    Content = "Who wrote 'Romeo and Juliet'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "William Shakespeare", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Charles Dickens", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Jane Austen", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Mark Twain", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 4,
                    QuizId = 1,
                    Content = "Which of the following are primary colors?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Red", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Blue", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Green", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Yellow", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 5,
                    QuizId = 1,
                    Content = "Which ocean is the largest?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Pacific Ocean", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Atlantic Ocean", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Indian Ocean", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Arctic Ocean", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 6,
                    QuizId = 1,
                    Content = "Which of these animals are mammals?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Whale", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Shark", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Bat", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Frog", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 7,
                    QuizId = 1,
                    Content = "What is the smallest prime number?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "2", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "1", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "0", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "3", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 8,
                    QuizId = 1,
                    Content = "Select all countries located in South America.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Brazil", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Argentina", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mexico", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Peru", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 9,
                    QuizId = 1,
                    Content = "What year did the Titanic sink?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "1912", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "1905", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1920", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1898", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 10,
                    QuizId = 1,
                    Content = "Which of the following are programming languages?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Python", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "HTML", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Java", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "CSS", IsCorrect = false }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 11,
                    QuizId = 2,
                    Content = "What is the chemical symbol for gold?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Au", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Ag", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Gd", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Go", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 12,
                    QuizId = 2,
                    Content = "Which of the following are states of matter?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Solid", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Liquid", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Plasma", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Granular", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 13,
                    QuizId = 2,
                    Content = "What planet is known as the Red Planet?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Mars", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Venus", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Jupiter", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Neptune", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 14,
                    QuizId = 2,
                    Content = "Which of the following are components of the cell?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Nucleus", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Chloroplast", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mitochondria", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Glucose", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 15,
                    QuizId = 2,
                    Content = "Which gas do plants primarily use for photosynthesis?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Carbon Dioxide", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Oxygen", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Hydrogen", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Nitrogen", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 16,
                    QuizId = 2,
                    Content = "Select the examples of renewable energy sources.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Solar", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Wind", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Coal", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Hydropower", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 17,
                    QuizId = 2,
                    Content = "What part of the atom has a positive charge?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Proton", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Electron", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Neutron", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Nucleus", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 18,
                    QuizId = 2,
                    Content = "Which of these are examples of amphibians?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Frog", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Salamander", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Lizard", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Toad", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 19,
                    QuizId = 2,
                    Content = "What is the boiling point of water at sea level?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "100°C", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "90°C", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "80°C", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "120°C", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 20,
                    QuizId = 2,
                    Content = "Select all planets that have rings.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Saturn", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Jupiter", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Uranus", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mars", IsCorrect = false }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 21,
                    QuizId = 3,
                    Content = "Who was the first President of the United States?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "George Washington", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Thomas Jefferson", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Abraham Lincoln", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "John Adams", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 22,
                    QuizId = 3,
                    Content = "Which ancient civilizations built pyramids?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Egyptians", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mayans", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Romans", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Aztecs", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 23,
                    QuizId = 3,
                    Content = "In what year did World War II end?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "1945", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "1939", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1941", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1950", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 24,
                    QuizId = 3,
                    Content = "Which of these explorers sailed around the world?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Ferdinand Magellan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Francis Drake", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Marco Polo", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "James Cook", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 25,
                    QuizId = 3,
                    Content = "The Berlin Wall fell in what year?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "1989", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "1991", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1985", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "1990", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 26,
                    QuizId = 3,
                    Content = "Which empires once ruled over large parts of Europe?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Roman Empire", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Ottoman Empire", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Ming Dynasty", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Byzantine Empire", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 27,
                    QuizId = 3,
                    Content = "Who was known as the 'Maid of Orléans'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Joan of Arc", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Marie Antoinette", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Catherine the Great", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Elizabeth I", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 28,
                    QuizId = 3,
                    Content = "Which countries were part of the Axis powers in World War II?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Germany", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Italy", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Japan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "France", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 29,
                    QuizId = 3,
                    Content = "Who was the British Prime Minister during most of World War II?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Winston Churchill", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Neville Chamberlain", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Clement Attlee", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Tony Blair", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 30,
                    QuizId = 3,
                    Content = "Select events that took place during the 20th century.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "The Moon Landing", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Cold War", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "French Revolution", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Fall of the Soviet Union", IsCorrect = true }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
    new Question
    {
        Id = 31,
        QuizId = 4,
        Content = "Which is the longest river in the world?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Nile", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Amazon", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Yangtze", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Mississippi", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 32,
        QuizId = 4,
        Content = "Which countries are landlocked?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Nepal", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Switzerland", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Thailand", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Austria", IsCorrect = true }
            })
    },
    new Question
    {
        Id = 33,
        QuizId = 4,
        Content = "What is the smallest country in the world by area?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Vatican City", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Monaco", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "San Marino", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Liechtenstein", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 34,
        QuizId = 4,
        Content = "Select the countries that span more than one continent.",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Russia", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Turkey", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Egypt", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Canada", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 35,
        QuizId = 4,
        Content = "What is the capital of Canada?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Ottawa", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Toronto", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Vancouver", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Montreal", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 36,
        QuizId = 4,
        Content = "Which continents lie entirely in the Southern Hemisphere?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Australia", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Antarctica", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Africa", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "South America", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 37,
        QuizId = 4,
        Content = "Mount Everest lies in which mountain range?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Himalayas", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Andes", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Alps", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Rockies", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 38,
        QuizId = 4,
        Content = "Select all countries through which the Equator passes.",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Ecuador", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Kenya", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Brazil", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Mexico", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 39,
        QuizId = 4,
        Content = "What desert is the largest in the world by area?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Antarctic Desert", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Sahara", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Gobi", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "Arctic", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 40,
        QuizId = 4,
        Content = "Which of these are island nations?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "Japan", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Iceland", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "Norway", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "New Zealand", IsCorrect = true }
            })
    }
);
            modelBuilder.Entity<Question>().HasData(
    new Question
    {
        Id = 41,
        QuizId = 5,
        Content = "What is the value of π (pi) rounded to two decimal places?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "3.14", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "2.71", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "1.61", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "3.41", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 42,
        QuizId = 5,
        Content = "Which of the following numbers are prime?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "2", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "3", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "4", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "5", IsCorrect = true }
            })
    },
    new Question
    {
        Id = 43,
        QuizId = 5,
        Content = "What is the square root of 144?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "12", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "14", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "11", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "10", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 44,
        QuizId = 5,
        Content = "Select all even numbers.",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "6", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "11", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "14", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "9", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 45,
        QuizId = 5,
        Content = "What is 7 × 8?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "56", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "64", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "48", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "52", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 46,
        QuizId = 5,
        Content = "Which of these are multiples of 3?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "9", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "12", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "14", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "15", IsCorrect = true }
            })
    },
    new Question
    {
        Id = 47,
        QuizId = 5,
        Content = "What is the value of 3²?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "9", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "6", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "8", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "12", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 48,
        QuizId = 5,
        Content = "Select all factors of 24.",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "2", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "3", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "5", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "8", IsCorrect = true }
            })
    },
    new Question
    {
        Id = 49,
        QuizId = 5,
        Content = "Which number is both a square and a cube?",
        QuestionType = QuestionType.SingleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "64", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "16", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "81", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "100", IsCorrect = false }
            })
    },
    new Question
    {
        Id = 50,
        QuizId = 5,
        Content = "Which of the following equations are true?",
        QuestionType = QuestionType.MultipleChoice,
        OptionsJson = JsonHelper.ConvertToAnswerJson(
            new List<QuestionOptionsSnapshot>
            {
                new QuestionOptionsSnapshot { Content = "5 + 3 = 8", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "10 ÷ 2 = 4", IsCorrect = false },
                new QuestionOptionsSnapshot { Content = "6 × 6 = 36", IsCorrect = true },
                new QuestionOptionsSnapshot { Content = "7 - 2 = 6", IsCorrect = false }
            })
    }
);
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 51,
                    QuizId = 6,
                    Content = "Who wrote '1984'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "George Orwell", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Aldous Huxley", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Ray Bradbury", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Ernest Hemingway", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 52,
                    QuizId = 6,
                    Content = "Which of the following are works by William Shakespeare?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Hamlet", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Macbeth", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Frankenstein", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Othello", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 53,
                    QuizId = 6,
                    Content = "Who is the author of 'Pride and Prejudice'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Jane Austen", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Charlotte Brontë", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Emily Dickinson", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Mary Shelley", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 54,
                    QuizId = 6,
                    Content = "Select all Greek epic poems.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "The Iliad", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Odyssey", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Beowulf", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "The Aeneid", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 55,
                    QuizId = 6,
                    Content = "What is the real name of Mark Twain?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Samuel Clemens", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Theodore Dreiser", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "F. Scott Fitzgerald", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Henry James", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 56,
                    QuizId = 6,
                    Content = "Which characters appear in 'The Great Gatsby'?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Jay Gatsby", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Daisy Buchanan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Tom Buchanan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Holden Caulfield", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 57,
                    QuizId = 6,
                    Content = "Who wrote the poem 'The Raven'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Edgar Allan Poe", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Robert Frost", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Walt Whitman", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Emily Dickinson", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 58,
                    QuizId = 6,
                    Content = "Which of the following are literary genres?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Tragedy", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Satire", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Suspense", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Cartography", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 59,
                    QuizId = 6,
                    Content = "Who wrote 'To Kill a Mockingbird'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Harper Lee", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Toni Morrison", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "J.D. Salinger", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "John Steinbeck", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 60,
                    QuizId = 6,
                    Content = "Select all novels set in dystopian futures.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "1984", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Brave New World", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Handmaid's Tale", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Catcher in the Rye", IsCorrect = false }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 61,
                    QuizId = 7,
                    Content = "Who painted the Mona Lisa?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Leonardo da Vinci", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Michelangelo", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Raphael", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Vincent van Gogh", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 62,
                    QuizId = 7,
                    Content = "Which of the following are characteristics of Baroque art?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Dramatic lighting", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Emotional intensity", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Flat perspective", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Rich, deep color", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 63,
                    QuizId = 7,
                    Content = "In which country did Impressionism originate?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "France", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Italy", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Germany", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Spain", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 64,
                    QuizId = 7,
                    Content = "Which artists were part of the Cubist movement?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Pablo Picasso", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Georges Braque", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Claude Monet", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Henri Matisse", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 65,
                    QuizId = 7,
                    Content = "What is the name of Michelangelo's famous sculpture housed in the Vatican?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Pietà", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "David", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Laocoön and His Sons", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "The Thinker", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 66,
                    QuizId = 7,
                    Content = "Which of the following are famous works of Surrealism?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "The Persistence of Memory", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Elephants", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Water Lilies", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "The Treachery of Images", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 67,
                    QuizId = 7,
                    Content = "Who painted 'The Starry Night'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Vincent van Gogh", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Edvard Munch", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Paul Cézanne", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Henri Rousseau", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 68,
                    QuizId = 7,
                    Content = "Select all artworks that are housed in the Louvre Museum.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Mona Lisa", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Liberty Leading the People", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "The Birth of Venus", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "The Raft of the Medusa", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 69,
                    QuizId = 7,
                    Content = "Which artist is known for cutting off his own ear?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Vincent van Gogh", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Paul Gauguin", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Édouard Manet", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Gustav Klimt", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 70,
                    QuizId = 7,
                    Content = "Which of the following are considered Renaissance artists?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Leonardo da Vinci", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Michelangelo", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Sandro Botticelli", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Salvador Dalí", IsCorrect = false }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 71,
                    QuizId = 8,
                    Content = "Which technology is widely used in virtual assistants like Alexa and Siri?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Natural Language Processing", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Blockchain", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Quantum Computing", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "3D Printing", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 72,
                    QuizId = 8,
                    Content = "Which of these are considered part of Industry 4.0?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Internet of Things (IoT)", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Artificial Intelligence", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Assembly Lines (Industry 2.0)", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Cloud Computing", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 73,
                    QuizId = 8,
                    Content = "What is the primary purpose of blockchain technology?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Decentralized record-keeping", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Faster internet speeds", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Quantum encryption", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Real-time image rendering", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 74,
                    QuizId = 8,
                    Content = "Which technologies are used in renewable energy systems?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Solar Panels", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Wind Turbines", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Diesel Generators", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Hydroelectric Dams", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 75,
                    QuizId = 8,
                    Content = "What does GPT in ChatGPT stand for?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Generative Pre-trained Transformer", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "General Purpose Technology", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Global Processing Tool", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Graphic Performance Tracker", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 76,
                    QuizId = 8,
                    Content = "Which of these are applications of generative AI?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Image generation", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Music composition", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Social media moderation", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Text summarization", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 77,
                    QuizId = 8,
                    Content = "Which company developed the first iPhone?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Apple", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Google", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Microsoft", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Samsung", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 78,
                    QuizId = 8,
                    Content = "Which of the following are wearable technologies?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Smartwatches", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Fitness Trackers", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Laptops", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "AR Glasses", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 79,
                    QuizId = 8,
                    Content = "What does 5G primarily improve over 4G?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Network speed and latency", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Display resolution", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Battery life", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Color accuracy", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 80,
                    QuizId = 8,
                    Content = "Which of these are examples of edge computing devices?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Smart thermostats", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Self-driving cars", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Cloud data centers", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Drones with onboard AI", IsCorrect = true }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 81,
                    QuizId = 9,
                    Content = "Which country is famous for the tea ceremony known as Chanoyu?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Japan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "China", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "India", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Thailand", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 82,
                    QuizId = 9,
                    Content = "Which of the following are traditional forms of dance?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Flamenco", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Ballet", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Samba", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Karate", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 83,
                    QuizId = 9,
                    Content = "Diwali is a major festival celebrated in which country?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "India", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Pakistan", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Nepal", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Sri Lanka", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 84,
                    QuizId = 9,
                    Content = "Which of the following are official languages of the United Nations?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "English", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Arabic", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Portuguese", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Russian", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 85,
                    QuizId = 9,
                    Content = "Which country is known for the cultural tradition of wearing kilts?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Scotland", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Ireland", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Wales", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Norway", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 86,
                    QuizId = 9,
                    Content = "Which of the following are traditional cuisines?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Sushi (Japan)", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Tacos (Mexico)", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Pizza (Italy)", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Poutine (Australia)", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 87,
                    QuizId = 9,
                    Content = "Carnival is a major cultural event famously held in which Brazilian city?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Rio de Janeiro", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "São Paulo", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Brasília", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Salvador", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 88,
                    QuizId = 9,
                    Content = "Which of the following are elements of traditional Chinese culture?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Calligraphy", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Tai Chi", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Manga", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Dragon Dance", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 89,
                    QuizId = 9,
                    Content = "What is the primary religion practiced in Thailand?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Buddhism", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Islam", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Christianity", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Hinduism", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 90,
                    QuizId = 9,
                    Content = "Select festivals that celebrate the lunar calendar.",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Lunar New Year", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mid-Autumn Festival", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Thanksgiving", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Eid al-Fitr", IsCorrect = true }
                        })
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 91,
                    QuizId = 10,
                    Content = "Which country has won the most FIFA World Cup titles?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Brazil", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Germany", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Argentina", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Italy", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 92,
                    QuizId = 10,
                    Content = "Which of the following are Olympic events?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Swimming", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Gymnastics", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Cricket", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Track and Field", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 93,
                    QuizId = 10,
                    Content = "In which sport is the term 'love' used to indicate a score of zero?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Tennis", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Badminton", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Squash", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Table Tennis", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 94,
                    QuizId = 10,
                    Content = "Which of these are considered major golf championships?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "The Masters", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "US Open", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Wimbledon", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "The Open Championship", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 95,
                    QuizId = 10,
                    Content = "Which country hosted the 2020 Summer Olympics (held in 2021)?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Japan", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "China", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Brazil", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "United Kingdom", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 96,
                    QuizId = 10,
                    Content = "Which of these sports involve a ball?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Basketball", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Volleyball", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Judo", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Cricket", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 97,
                    QuizId = 10,
                    Content = "Which famous boxer was known as 'The Greatest'?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Muhammad Ali", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Mike Tyson", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Floyd Mayweather", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Joe Frazier", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 98,
                    QuizId = 10,
                    Content = "Which of the following are winter sports?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Skiing", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Snowboarding", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Surfing", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "Figure Skating", IsCorrect = true }
                        })
                },
                new Question
                {
                    Id = 99,
                    QuizId = 10,
                    Content = "How many players are on a soccer team on the field (per side)?",
                    QuestionType = QuestionType.SingleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "11", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "10", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "12", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "9", IsCorrect = false }
                        })
                },
                new Question
                {
                    Id = 100,
                    QuizId = 10,
                    Content = "Which of the following sports originated in the USA?",
                    QuestionType = QuestionType.MultipleChoice,
                    OptionsJson = JsonHelper.ConvertToAnswerJson(
                        new List<QuestionOptionsSnapshot>
                        {
                            new QuestionOptionsSnapshot { Content = "Basketball", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Baseball", IsCorrect = true },
                            new QuestionOptionsSnapshot { Content = "Rugby", IsCorrect = false },
                            new QuestionOptionsSnapshot { Content = "American Football", IsCorrect = true }
                        })
                }
            );
        }
    }
}