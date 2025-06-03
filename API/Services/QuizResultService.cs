using API.Dtos.QuizResult;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class QuizResultService : IQuizResultService
    {
        private readonly IQuizResultRepository _quizResultRepository;
        private readonly IQuizRepository _quizRepository;

        public QuizResultService(IQuizResultRepository quizResultRepository, IQuizRepository quizRepository)
        {
            _quizResultRepository = quizResultRepository;
            _quizRepository = quizRepository;
        }

        private bool IsQuestionCorrect(Question question, QuizResult quizResult)
        {
            var subnmittedAnswers = quizResult.SelectedAnswers
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
                    return true;
                }
            }
            return false;
        }

        public async Task<QuizResultDto> GetQuizResult(int quizResultId)
        {
            var quizResult = await _quizResultRepository.GetQuizResultById(quizResultId);
            var questionList = await _quizRepository.GetQuestionsOfQuiz(quizResult.QuizId);

            QuizResultDto quizResultDto = new QuizResultDto
            {
                Id = quizResult.Id,
                QuizId = quizResult.QuizId,
                QuizName = quizResult.Quiz.Name,
                UserId = quizResult.UserId,
                TotalScore = quizResult.Score,
            };

            quizResultDto.Questions = questionList.Select(q => new ResultQuestionDto
            {
                QuestionId = q.Id,
                QuestionContent = q.Content,
                IsCorrect = IsQuestionCorrect(q, quizResult),
                Answers = questionList.Where(question => question.Id == q.Id)
                    .SelectMany(question => question.Answers.Select(a => new ResultAnswerDto
                    {
                        AnswerId = a.Id,
                        Content = a.Content,
                        IsCorrectAnswer = a.IsCorrect,
                        UserSelected = quizResult.SelectedAnswers.Any(ans => ans.QuestionId == q.Id && ans.AnswerId == a.Id)
                    })).ToList()
            }).ToList();

            return quizResultDto;
        }
    }
}
