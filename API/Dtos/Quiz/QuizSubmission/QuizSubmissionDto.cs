﻿namespace API.Dtos.Quiz.QuizSubmission
{
    public class QuizSubmissionDto
    {
        public int QuizId { get; set; }
        public List<SubmissionAnswerDto> Answers { get; set; } = new List<SubmissionAnswerDto>();
    }
}
