using API.Dtos.Quiz;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class Admin_QuestionService : Admin_IQuestionService
    {
        private readonly Admin_IQuestionRepository _repo;

        public Admin_QuestionService(Admin_IQuestionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Admin_QuestionDto>> GetQuestionsByQuizId(int quizId)
        {
            var questions = await _repo.GetQuestionsByQuizId(quizId);
            return questions.Select(q => new Admin_QuestionDto
            {
                Id = q.Id,
                Content = q.Content,
                QuizId = q.QuizId,
                OptionsJson = q.OptionsJson,
                QuestionType = q.QuestionType.ToString()
            });
        }

        public async Task<Admin_QuestionDto?> GetById(int id)
        {
            var q = await _repo.GetById(id);
            if (q == null) return null;
            return new Admin_QuestionDto
            {
                Id = q.Id,
                Content = q.Content,
                QuizId = q.QuizId,
                OptionsJson = q.OptionsJson,
                QuestionType = q.QuestionType.ToString()
            };
        }

        public async Task Add(Admin_QuestionDto dto)
        {
            var question = new Question
            {
                Content = dto.Content,
                QuizId = dto.QuizId,
                OptionsJson = dto.OptionsJson,
                QuestionType = Enum.Parse<API.Models.Enums.QuestionType>(dto.QuestionType, true)
            };
            await _repo.Add(question);
        }

        public async Task Update(Admin_QuestionDto dto)
        {
            var question = await _repo.GetById(dto.Id);
            if (question == null) throw new Exception("Question not found");
            question.Content = dto.Content;
            question.OptionsJson = dto.OptionsJson;
            question.QuestionType = Enum.Parse<API.Models.Enums.QuestionType>(dto.QuestionType, true);

            await _repo.Update(question);
        }

        public async Task Delete(int id)
        {
            var question = await _repo.GetById(id);
            if (question == null) throw new Exception("Question not found");
            await _repo.Delete(question);
        }
    }
}
