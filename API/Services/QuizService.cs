using API.Dtos.Quiz.QuizSubmission;
using API.Models;
using API.Repositories;

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
    }
}
