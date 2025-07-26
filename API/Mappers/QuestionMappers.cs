using API.Dtos.Question;
using API.Models;

namespace API.Mappers
{
    public static class QuestionMappers
    {
        public static QuestionDto ToQuestionDto(this Question question)
        {
            return new QuestionDto
            {
                Id = question.Id,
                Content = question.Content,
                OptionsJson = question.OptionsJson,
                QuestionType = question.QuestionType,
                IsActive = question.IsActive,
                QuizId = question.QuizId,
                QuizName = question.Quiz?.Name
            };
        }



        public static Question ToQuestion(this QuestionDto questionDto)
        {
            return new Question
            {
                Id = questionDto.Id,
                Content = questionDto.Content,
                OptionsJson = questionDto.OptionsJson,
                QuestionType = questionDto.QuestionType,
                IsActive = questionDto.IsActive,
                QuizId = questionDto.QuizId,
                Quiz = new Quiz { Id = questionDto.QuizId }
            };


        }
    }
}
