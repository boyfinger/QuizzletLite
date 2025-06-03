using API.Dtos.Quiz.QuizSubmission;

namespace API.Services
{
    public interface IQuizService
    {
        Task<double> ProcessQuizAttempt(QuizSubmissionDto submissionDto);
    }
}
