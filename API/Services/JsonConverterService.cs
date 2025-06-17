using API.Models.Snapshots;
using Newtonsoft.Json;

namespace API.Services
{
    public class JsonConverterService : IJsonConverterService
    {
        public List<QuestionOptionsSnapshot> ConvertFromAnswerJson(string json)
        {
            return JsonConvert.DeserializeObject<List<QuestionOptionsSnapshot>>(json) ?? new List<QuestionOptionsSnapshot>();
        }

        public QuizAttemptAnswersSnapshot ConvertFromQuizAttemptQuestionsSnapshotJson(string json)
        {
            return JsonConvert.DeserializeObject<QuizAttemptAnswersSnapshot>(json) ?? new QuizAttemptAnswersSnapshot();
        }

        public string ConvertToAnswerJson(List<QuestionOptionsSnapshot> answers)
        {
            return JsonConvert.SerializeObject(answers);
        }

        public string ConvertToQuizAttemptQuestionsSnapshotJson(QuizAttemptAnswersSnapshot snapshot)
        {
            return JsonConvert.SerializeObject(snapshot);
        }
    }
}
