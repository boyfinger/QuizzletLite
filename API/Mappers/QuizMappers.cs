﻿using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizDetails;
using API.Helpers;
using API.Models;
using System.Linq.Expressions;

namespace API.Mappers
{
    public static class QuizMappers
    {
        public static QuizDto ToQuizDto(this Quiz quiz)
        {
            return new QuizDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedByUserName = quiz.CreatedByNavigation?.Username ?? "Unknown",
                NumberOfQuestions = quiz.Questions.Count
            };
        }

        public static QuizDetailsDto ToQuizDetailsDto(this Quiz quiz)
        {
            return new QuizDetailsDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedByUsername = quiz.CreatedByNavigation?.Username ?? "Unknown",
                Questions = quiz.Questions
                    .Select(q => new QuizQuestionsDto
                    {
                        Id = q.Id,
                        Content = q.Content,
                        QuestionType = q.QuestionType,
                        Options = JsonHelper.ConvertFromAnswerJson(q.OptionsJson)
                            .Select(a => new QuestionOptionDto
                            {
                                Content = a.Content,
                                IsCorrect = a.IsCorrect
                            }).ToList()
                    }).ToList()
            };
        }

        public static QuizzesDto MapToDto(Quiz quiz)
        {
            return new QuizzesDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedBy = quiz.CreatedBy,
                CreatedOn = quiz.CreatedOn,
                IsActive = quiz.IsActive
            };
        }

        public static Expression<Func<Quiz, QuizzesDto>> MapToDtoExpr => quiz => new QuizzesDto
        {
            Id = quiz.Id,
            Name = quiz.Name,
            CreatedBy = quiz.CreatedBy,
            CreatedOn = quiz.CreatedOn,
            IsActive = quiz.IsActive
        };
    }
}
