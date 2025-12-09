using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizDetails;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Mappers;
using API.Models;
using API.Models.Snapshots;
using API.Repositories;

namespace API.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizAttemptRepository _quizAttemptRepository;

        public QuizService(IQuizRepository quizRepository, IQuizAttemptRepository quizAttemptRepository)
        {
            _quizRepository = quizRepository;
            _quizAttemptRepository = quizAttemptRepository;
        }

        private double CalculateScore(QuizSubmissionDto submissionDto, ICollection<Question> questionList)
        {
            int correctAnswersCount = 0;

            foreach (SubmissionAnswerDto answerDto in submissionDto.Answers)
            {
                var question = questionList.FirstOrDefault(q => q.Id == answerDto.QuestionId);
                if (question != null)
                {
                    var questionOptions = JsonHelper.ConvertFromAnswerJson(question.OptionsJson);
                    var correctAnswers = questionOptions.Where(o => o.IsCorrect).Select(o => o.Content).ToList();

                    var selectedAnswers = answerDto.SelectedAnswers ?? new List<string>();
                    if (selectedAnswers.Count == correctAnswers.Count && !selectedAnswers.Except(correctAnswers).Any())
                    {
                        correctAnswersCount++;
                    }

                }
            }

            return ((double)correctAnswersCount / questionList.Count) * 10;
        }

        public async Task<QuizAttempt> ProcessQuizAttempt(QuizSubmissionDto submissionDto, int userId)
        {
            var quiz = await _quizRepository.GetQuizById(submissionDto.QuizId);
            double score = CalculateScore(submissionDto, quiz.Questions);

            var quizAttemptAnswers = new List<QuizAttemptAnswersSnapshot>();

            foreach (var question in quiz.Questions)
            {
                var questionOptions = JsonHelper.ConvertFromAnswerJson(question.OptionsJson);
                var attemptAnswers = new QuizAttemptAnswersSnapshot
                {
                    QuestionContent = question.Content,
                    Options = questionOptions,
                    SelectedAnswers = submissionDto.Answers
                        .FirstOrDefault(a => a.QuestionId == question.Id)?.SelectedAnswers ?? new List<string>()
                };
                quizAttemptAnswers.Add(attemptAnswers);
            }

            var quizAttempt = new QuizAttempt
            {
                UserId = userId,
                QuizId = submissionDto.QuizId,
                CompletedDate = DateTime.UtcNow,
                Score = score,
                QuizName = quiz.Name,
                AnswersJson = JsonHelper.ConvertToQuizAttemptQuestionsSnapshotJson(quizAttemptAnswers)
            };
            return await _quizAttemptRepository.SaveQuizAttempt(quizAttempt);
        }

        public async Task<QuizzesDto> CreateQuizAsync(CreateQuizDto createQuizDto)
        {
            var quiz = new Quiz
            {
                Name = createQuizDto.Name,
                CreatedBy = createQuizDto.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                IsActive = createQuizDto.IsActive
            };

            var createdQuiz = await _quizRepository.CreateQuiz(quiz);
            return QuizMappers.MapToDto(createdQuiz);
        }

        public async Task<QuizzesDto> UpdateQuizAsync(int quizId, UpdateQuizDto updateQuizDto)
        {
            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
            if (quiz == null) throw new Exception("Quiz not found");

            quiz.Name = updateQuizDto.Name;
            quiz.IsActive = updateQuizDto.IsActive;

            var updatedQuiz = await _quizRepository.UpdateQuiz(quiz);
            return QuizMappers.MapToDto(updatedQuiz);
        }

        public async Task<bool> DeactivateQuizAsync(int quizId)
        {
            return await _quizRepository.DeactivateQuiz(quizId);
        }

        public async Task<List<QuizzesDto>> GetQuizzesByUserAsync(int userId)
        {
            var quizzes = await _quizRepository.GetQuizzesByUser(userId);
            return quizzes.Select(QuizMappers.MapToDto).ToList();
        }
        public async Task<QuizzesDto> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
            if (quiz == null) throw new Exception("Quiz not found");
            return QuizMappers.MapToDto(quiz);
        }

        public async Task<QuizDetailsDto> GetQuizDetailsAsync(int quizId)
        {
            var quiz = await _quizRepository.GetQuizById(quizId);
            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            return new QuizDetailsDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedByUsername = quiz.CreatedByNavigation.Username,
                Questions = quiz.Questions.Select(q => new QuizQuestionsDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuestionType = q.QuestionType,
                    Options = JsonHelper.ConvertFromAnswerJson(q.OptionsJson).Select(o => new QuestionOptionDto
                    {
                        Content = o.Content,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<List<QuizzesDto>> GetUserQuizzesByPage(int userId, QuizQuery quizQuery)
        {
            var quizzes = await GetQuizzesByUserAsync(userId);
            var filtered = quizzes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(quizQuery.Name))
            {
                filtered = filtered.Where(q => q.Name.Contains(quizQuery.Name, StringComparison.OrdinalIgnoreCase));
            }
            if (quizQuery.IsActive.HasValue)
            {
                filtered = filtered.Where(q => q.IsActive == quizQuery.IsActive.Value);
            }
            var page = quizQuery.Page > 0 ? quizQuery.Page : 1;
            var pageSize = quizQuery.PageSize > 0 ? quizQuery.PageSize : 5;

            var pagedResult = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedResult;
        }

        public IQueryable<QuizzesDto> GetAllQuizzes()
        {
            return _quizRepository.GetAllQuizzes()
                .Select(QuizMappers.MapToDto).AsQueryable();
        }

        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            return await _quizRepository.DeactivateQuiz(quizId);
        }
    }
}
