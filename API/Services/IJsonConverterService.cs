using API.Models.Snapshots;
using Newtonsoft.Json;

namespace API.Services
{
    public interface IJsonConverterService
    {
        string ConvertToAnswerJson(List<QuestionOptionsSnapshot> answers);

        List<QuestionOptionsSnapshot> ConvertFromAnswerJson(string json);

        string ConvertToQuizAttemptQuestionsSnapshotJson(QuizAttemptAnswersSnapshot snapshot);

        QuizAttemptAnswersSnapshot ConvertFromQuizAttemptQuestionsSnapshotJson(string json);
    }
}
