using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizDetails;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Models;

namespace API.Services
{
    public interface IQuizService
    {
        Task<QuizAttempt> ProcessQuizAttempt(QuizSubmissionDto submissionDto, int userId);
        Task<QuizzesDto> CreateQuizAsync(CreateQuizDto createQuizDto);
        Task<QuizzesDto> UpdateQuizAsync(int quizId, UpdateQuizDto updateQuizDto);
        Task<bool> DeactivateQuizAsync(int quizId);
        Task<QuizzesDto> GetQuizByIdAsync(int quizId);
        Task<List<QuizzesDto>> GetQuizzesByUserAsync(int userId);
        Task<QuizDetailsDto> GetQuizDetailsAsync(int quizId);
        Task<List<QuizzesDto>> GetUserQuizzesByPage(int userId, QuizQuery quizQuery);

    }
}
