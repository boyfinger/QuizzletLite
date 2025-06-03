using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizSubmission;
using API.Models;
using API.Repositories;
using System.Linq;

namespace API.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        private double CalculateScore(QuizSubmissionDto submissionDto, List<Question> questionList)
        {
            int correctAnswersCount = 0;

            foreach (Question question in questionList)
            {
                var subnmittedAnswers = submissionDto.Answers
                    .Where(a => a.QuestionId == question.Id)
                    .Select(a => a.AnswerId)
                    .ToList();
                if (subnmittedAnswers != null && subnmittedAnswers.Count != 0)
                {
                    var correctAnswers = question.Answers
                        .Where(a => a.IsCorrect)
                        .Select(a => a.Id)
                        .ToList();
                    if (correctAnswers.OrderBy(id => id).SequenceEqual(subnmittedAnswers.OrderBy(id => id)))
                    {
                        correctAnswersCount++;
                    }
                }
            }

            return (correctAnswersCount / questionList.Count) * 10;
        }

        public async Task<double> ProcessQuizAttempt(QuizSubmissionDto submissionDto)
        {
            var questionList = await _quizRepository.GetQuestionsOfQuiz(submissionDto.QuizId);
            double score = CalculateScore(submissionDto, questionList);
            bool isSaved = await _quizRepository.SaveQuizAttempt(submissionDto, score);
            if (!isSaved)
            {
                throw new Exception("Failed to save quiz attempt.");
            }

            return score;
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
            return MapToDto(createdQuiz);
        }

        public async Task<QuizzesDto> UpdateQuizAsync(int quizId, UpdateQuizDto updateQuizDto)
        {
            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
            if (quiz == null) throw new Exception("Quiz not found");

            quiz.Name = updateQuizDto.Name;
            quiz.IsActive = updateQuizDto.IsActive;

            var updatedQuiz = await _quizRepository.UpdateQuiz(quiz);
            return MapToDto(updatedQuiz);
        }

        public async Task<bool> DeactivateQuizAsync(int quizId)
        {
            return await _quizRepository.DeactivateQuiz(quizId);
        }

        public async Task<List<QuizzesDto>> GetQuizzesByUserAsync(int userId)
        {
            var quizzes = await _quizRepository.GetQuizzesByUser(userId);
            return quizzes.Select(MapToDto).ToList();
        }
        public async Task<QuizzesDto> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _quizRepository.GetQuizByIdWithDetails(quizId);
            if (quiz == null) throw new Exception("Quiz not found");
            return MapToDto(quiz);
        }
        private QuizzesDto MapToDto(Quiz quiz)
        {
            return new QuizzesDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedBy = quiz.CreatedBy,
                CreatedOn = quiz.CreatedOn,
                IsActive = quiz.IsActive
            };
        }

        
    }
}
