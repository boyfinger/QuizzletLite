using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Models;

namespace API.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQuizzes(QuizQuery query);
        Task<List<Question>> GetQuestionsOfQuiz(int quizId);
        Task<bool> SaveQuizAttempt(QuizSubmissionDto submissionDto, double score);
        Task<Quiz?> GetQuizById(int quizId);
    }
}
