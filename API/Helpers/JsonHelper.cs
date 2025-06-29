using API.Models.Snapshots;
using Newtonsoft.Json;

namespace API.Helpers
{
    public static class JsonHelper
    {
        public static List<QuestionOptionsSnapshot> ConvertFromAnswerJson(string json)
        {
            return JsonConvert.DeserializeObject<List<QuestionOptionsSnapshot>>(json) ?? new List<QuestionOptionsSnapshot>();
        }

        public static List<QuizAttemptAnswersSnapshot> ConvertFromQuizAttemptQuestionsSnapshotJson(string json)
        {
            return JsonConvert.DeserializeObject<List<QuizAttemptAnswersSnapshot>>(json) ?? new List<QuizAttemptAnswersSnapshot>();
        }

        public static string ConvertToAnswerJson(List<QuestionOptionsSnapshot> answers)
        {
            return JsonConvert.SerializeObject(answers);
        }

        public static string ConvertToQuizAttemptQuestionsSnapshotJson(List<QuizAttemptAnswersSnapshot> snapshot)
        {
            return JsonConvert.SerializeObject(snapshot);
        }
    }
}
