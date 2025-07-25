using API.Dtos.QuizAttempt;
using API.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace API.Mappers
{
    public static class QuizAttemptMappers
    {
        public static QuizAttemptDto ToQuizAttemptDtoFull(this QuizAttempt entity)
        {
            return new QuizAttemptDto
            {
                Id = entity.Id,
                QuizId = entity.QuizId,
                QuizName = entity.QuizName,
                UserId = entity.UserId,
                TotalScore = entity.Score,
                CompletedDate = entity.CompletedDate,
                Questions = ParseAnswersJson(entity.AnswersJson)
            };
        }

        public static QuizAttemptDto ToQuizAttemptDtoLite(this QuizAttempt entity)
        {
            return new QuizAttemptDto
            {
                Id = entity.Id,
                QuizId = entity.QuizId,
                QuizName = entity.QuizName,
                UserId = entity.UserId,
                TotalScore = entity.Score,
                CompletedDate = entity.CompletedDate
            };
        }

        public static QuizAttempt ToQuizAttempt(this QuizAttemptDto dto)
        {
            return new QuizAttempt
            {
                Id = dto.Id,
                QuizId = dto.QuizId,
                UserId = dto.UserId,
                QuizName = dto.QuizName,
                Score = dto.TotalScore,
                AnswersJson = JsonConvert.SerializeObject(dto.Questions),
                CompletedDate = dto.CompletedDate // Có thể tùy chỉnh nếu cần
            };
        }

        private static List<AttemptQuestionDto> ParseAnswersJson(string answersJson)
        {
            if (string.IsNullOrWhiteSpace(answersJson))
                return new List<AttemptQuestionDto>();

            try
            {
                return JsonConvert.DeserializeObject<List<AttemptQuestionDto>>(answersJson) ?? new List<AttemptQuestionDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi parse AnswersJson: {ex.Message}");
                return new List<AttemptQuestionDto>();
            }
        }

        public static Expression<Func<QuizAttempt, QuizAttemptDto>> MapToQuizAttemptEpr => quizAttempt => new QuizAttemptDto
        {
            Id = quizAttempt.Id,
            QuizId = quizAttempt.QuizId,
            UserId = quizAttempt.UserId,
            QuizName = quizAttempt.QuizName,
            TotalScore = quizAttempt.Score,
            CompletedDate = quizAttempt.CompletedDate,
            Questions = ParseAnswersJson(quizAttempt.AnswersJson)
        };

        public static Expression<Func<QuizAttempt, QuizAttemptDto>> MapToQuizAttemptLiteExpr =>
            entity => new QuizAttemptDto
            {
                Id = entity.Id,
                QuizId = entity.QuizId,
                QuizName = entity.QuizName,
                UserId = entity.UserId,
                TotalScore = entity.Score,
                CompletedDate = entity.CompletedDate
            };
    }
}
