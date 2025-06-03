﻿using API.DAO;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class Admin_QuizRepository : Admin_IQuizRepository
    {
        private readonly QuizletLiteContext _context;

        public Admin_QuizRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<List<Quiz>> GetQuizzes(QuizQuery query)
        {
            var quizzes = _context.Quizzes.Include(q => q.CreatedByNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
                quizzes = quizzes.Where(q => q.Name.Contains(query.Name));

            return await quizzes
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizById(int id)
        {
            return await _context.Quizzes
                .Include(q => q.CreatedByNavigation)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddQuiz(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> QuizExists(int id)
        {
            return await _context.Quizzes.AnyAsync(q => q.Id == id);
        }
    }
}
