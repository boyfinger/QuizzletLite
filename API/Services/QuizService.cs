//using API.Dtos;
//using API.Dtos.Quiz;
//using API.Dtos.Quiz.QuizDetails;
//using API.Dtos.Quiz.QuizSubmission;
//using API.Helpers;
//using API.Models;
//using API.Models.Enums;
//using API.Models.Snapshots;
//using API.Repositories;
//using Newtonsoft.Json;

//namespace API.Services
//{
//    public class QuizService : IQuizService
//    {
//        private readonly IQuizRepository _quizRepository;

//        public QuizService(IQuizRepository quizRepository)
//        {
//            _quizRepository = quizRepository;
//        }

//        private double CalculateScore(QuizSubmissionDto submissionDto, ICollection<Question> questionList)
//        {
//            int correctAnswersCount = 0;

//            foreach (SubmissionAnswerDto answerDto in submissionDto.Answers)
//            {
//                var question = questionList.FirstOrDefault(q => q.Id == answerDto.QuestionId);
//                if (question != null)
//                {
//                    var questionOptions = JsonConverter.ConvertFromAnswerJson(question.OptionsJson);
//                    var correctAnswers = questionOptions.Where(o => o.IsCorrect).Select(o => o.Content).ToList();

//                    var selectedAnswers = answerDto.SelectedAnswers ?? new List<string>();
//                    if (selectedAnswers.Count == correctAnswers.Count && !selectedAnswers.Except(correctAnswers).Any())
//                    {
//                        correctAnswersCount++;
//                    }

//                }
//            }

//            return ((double)correctAnswersCount / questionList.Count) * 10;
//        }

//        public async Task<double> ProcessQuizAttempt(QuizSubmissionDto submissionDto, int userId)
//        {
//            var quiz = await _quizRepository.GetQuizById(submissionDto.QuizId);
//            double score = CalculateScore(submissionDto, quiz.Questions);

//            var quizAttemptAnswers = new List<QuizAttemptAnswersSnapshot>();

//            foreach (var question in quiz.Questions)
//            {
//                var questionOptions = JsonConverter.ConvertFromAnswerJson(question.OptionsJson);
//                var attemptAnswers = new QuizAttemptAnswersSnapshot
//                {
//                    QuestionContent = question.Content,
//                    Options = questionOptions,
//                    SelectedAnswers = submissionDto.Answers
//                        .FirstOrDefault(a => a.QuestionId == question.Id)?.SelectedAnswers ?? new List<string>()
//                };
//                quizAttemptAnswers.Add(attemptAnswers);
//            }

//            var quizAttempt = new QuizAttempt
//            {
//                UserId = userId,
//                QuizId = submissionDto.QuizId,
//                CompletedDate = DateTime.UtcNow,
//                Score = score,
//                QuizName = quiz.Name,
//                AnswersJson = JsonConverter.ConvertToQuizAttemptQuestionsSnapshotJson(quizAttemptAnswers)
//            };
//            bool isSaved = await _quizRepository.SaveQuizAttempt(quizAttempt);
//            if (!isSaved)
//            {
//                throw new Exception("Failed to save quiz attempt.");
//            }

//            return score;
//        }

//        public async Task<QuizzesDto> CreateQuizAsync(CreateQuizDto createQuizDto)
//        {
//            var quiz = new Quiz
//            {
//                Name = createQuizDto.Name,
//                CreatedBy = createQuizDto.CreatedBy,
//                CreatedOn = DateTime.UtcNow,
//                IsActive = createQuizDto.IsActive
//            };

//            var createdQuiz = await _quizRepository.CreateQuiz(quiz);
//            return MapToDto(createdQuiz);
//        }

//        public async Task<QuizzesDto> UpdateQuizAsync(int quizId, UpdateQuizDto updateQuizDto)
//        {
//            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
//            if (quiz == null) throw new Exception("Quiz not found");

//            quiz.Name = updateQuizDto.Name;
//            quiz.IsActive = updateQuizDto.IsActive;

//            var updatedQuiz = await _quizRepository.UpdateQuiz(quiz);
//            return MapToDto(updatedQuiz);
//        }

//        public async Task<bool> DeactivateQuizAsync(int quizId)
//        {
//            return await _quizRepository.DeactivateQuiz(quizId);
//        }

//        public async Task<List<QuizzesDto>> GetQuizzesByUserAsync(int userId)
//        {
//            var quizzes = await _quizRepository.GetQuizzesByUser(userId);
//            return quizzes.Select(MapToDto).ToList();
//        }
//        public async Task<QuizzesDto> GetQuizByIdAsync(int quizId)
//        {
//            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
//            if (quiz == null) throw new Exception("Quiz not found");
//            return MapToDto(quiz);
//        }
//        private QuizzesDto MapToDto(Quiz quiz)
//        {
//            return new QuizzesDto
//            {
//                Id = quiz.Id,
//                Name = quiz.Name,
//                CreatedBy = quiz.CreatedBy,
//                CreatedOn = quiz.CreatedOn,
//                IsActive = quiz.IsActive
//            };
//        }

//        public async Task<QuizDetailsDto> GetQuizDetailsAsync(int quizId)
//        {
//            var quiz = await _quizRepository.GetQuizById(quizId);
//            if (quiz == null)
//            {
//                throw new Exception("Quiz not found");
//            }
//            return new QuizDetailsDto
//            {
//                Id = quiz.Id,
//                Name = quiz.Name,
//                CreatedByUsername = quiz.CreatedByNavigation.Username,
//                Questions = quiz.Questions.Select(q => new QuizQuestionsDto
//                {
//                    Id = q.Id,
//                    Content = q.Content,
//                    QuestionType = q.QuestionType,
//                    Options = JsonConverter.ConvertFromAnswerJson(q.OptionsJson).Select(o => new QuestionOptionDto
//                    {
//                        Content = o.Content,
//                        IsCorrect = o.IsCorrect
//                    }).ToList()
//                }).ToList()
//            };
//        }
//    }
//}
