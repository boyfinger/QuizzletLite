using API.Dtos.QuizAttempt;
using API.Helpers;
using API.Mappers;
using API.Models.Snapshots;
using API.Repositories;

namespace API.Services
{
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly IQuizAttemptRepository _quizAttemptRepository;

        public QuizAttemptService(IQuizAttemptRepository quizResultRepository)
        {
            _quizAttemptRepository = quizResultRepository;
        }

        public IQueryable<QuizAttemptDto> GetAllQuizAttempts()
        {
            return _quizAttemptRepository.GetAllQuizAttempts().Select(QuizAttemptMappers.ToQuizAttemptDtoFull).AsQueryable();
        }

        public async Task<QuizAttemptDto> GetQuizAttempt(int quizAttemptId)
        {
            var quizAttempt = await _quizAttemptRepository.GetQuizAttemptById(quizAttemptId);
            if (quizAttempt == null)
            {
                throw new Exception($"Quiz attempt with ID {quizAttemptId} not found.");
            }
            return new QuizAttemptDto
            {
                Id = quizAttempt.Id,
                QuizId = quizAttempt.QuizId,
                QuizName = quizAttempt.Quiz.Name,
                UserId = quizAttempt.UserId,
                Questions = JsonHelper.ConvertFromQuizAttemptQuestionsSnapshotJson(quizAttempt.AnswersJson).Select(
                    q => new AttemptQuestionDto
                    {
                        QuestionContent = q.QuestionContent,
                        IsCorrect = IsQuestionAnsweredCorrect(q),
                        Answers = q.Options.Select(o => new AttemptAnswerDto
                        {
                            Content = o.Content,
                            IsCorrect = o.IsCorrect,
                            IsSelected = q.SelectedAnswers.Contains(o.Content)
                        }).ToList(),
                    }).ToList(),
                TotalScore = quizAttempt.Score
            };
        }

        private bool IsQuestionAnsweredCorrect(QuizAttemptAnswersSnapshot answers)
        {
            var correctAnswers = answers.Options
                .Where(o => o.IsCorrect)
                .Select(o => o.Content)
                .ToList();
            bool isCorrect = answers.SelectedAnswers.Count == correctAnswers.Count &&
                            answers.SelectedAnswers.OrderBy(x => x).SequenceEqual(correctAnswers.OrderBy(x => x));
            return isCorrect;
        }
    }
}
