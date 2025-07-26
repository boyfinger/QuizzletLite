using API.Dtos.Question;
using API.Models;

namespace API.Mappers
{
    public static class QuestionMappers
    {
        public static QuestionFullDto ToQuestionDto(this Question question)
        {
            return new QuestionFullDto
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



        public static Question ToQuestion(this QuestionFullDto questionDto)
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
