﻿using API.Dtos.Quiz;
using API.Dtos;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Mappers;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizService _quizService;

        public QuizController(IQuizRepository quizRepository, IQuizService quizService)
        {
            _quizRepository = quizRepository;
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes([FromQuery] QuizQuery query)
        {
            var listQuery = await _quizRepository.GetQuizzes(query);
            var list = listQuery.Select(q => q.ToQuizDto());
            return Ok(list);
        }

        [HttpPost("DoQuiz")]
        public async Task<IActionResult> DoQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            try
            {
                return Ok(await _quizService.ProcessQuizAttempt(submissionDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{quizId}/Questions")]
        public async Task<IActionResult> GetQuizDetails(int quizId)
        {
            var quiz = await _quizRepository.GetQuizById(quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            var quizDto = quiz.ToQuizDetailsDto();
            return Ok(quizDto);

        }

        [HttpPost]
        public async Task<ActionResult<QuizzesDto>> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
        {
            if (createQuizDto == null) return BadRequest("Quiz data is required");
            var quizDto = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuizById), new { id = quizDto.Id }, quizDto);
        }

        [HttpPut("{quizId}")]
        public async Task<ActionResult<QuizzesDto>> UpdateQuiz(int quizId, [FromBody] UpdateQuizDto updateQuizDto)
        {
            if (updateQuizDto == null) return BadRequest("Quiz data is required");
            try
            {
                var quizDto = await _quizService.UpdateQuizAsync(quizId, updateQuizDto);
                return Ok(quizDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPatch("{quizId}/deactivate")]
        public async Task<IActionResult> DeactivateQuiz(int quizId)
        {
            var success = await _quizService.DeactivateQuizAsync(quizId);
            if (!success) return NotFound("Quiz not found");
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<QuizzesDto>>> GetQuizzesByUser(int userId)
        {
            var quizzes = await _quizService.GetQuizzesByUserAsync(userId);
            return Ok(quizzes);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<double>> SubmitQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            if (submissionDto == null || submissionDto.Answers == null) return BadRequest("Submission data is required");
            var score = await _quizService.ProcessQuizAttempt(submissionDto);
            return Ok(score);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizzesDto>> GetQuizById(int id)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(id);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
