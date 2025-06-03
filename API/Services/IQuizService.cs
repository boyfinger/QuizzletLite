using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizSubmission;

namespace API.Services
{
    public interface IQuizService
    {
        Task<double> ProcessQuizAttempt(QuizSubmissionDto submissionDto);
        Task<QuizzesDto> CreateQuizAsync(CreateQuizDto createQuizDto);
        Task<QuizzesDto> UpdateQuizAsync(int quizId, UpdateQuizDto updateQuizDto);
        Task<bool> DeactivateQuizAsync(int quizId);
        Task<QuizzesDto> GetQuizByIdAsync(int quizId);
        Task<List<QuizzesDto>> GetQuizzesByUserAsync(int userId);
        
    }
}
