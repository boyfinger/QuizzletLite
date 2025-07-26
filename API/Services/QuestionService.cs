using API.Dtos.Question;
using API.Mappers;
using API.Repositories;

namespace API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public IQueryable<QuestionDto> GetAllQuestions()
        {
            var entities = _questionRepository.GetAllQuestions();
            return entities.Select(q => q.ToQuestionDto()).AsQueryable();
        }

        public async Task<QuestionDto?> GetByIdAsync(int id)
        {
            var entity = await _questionRepository.GetByIdAsync(id);
            return entity?.ToQuestionDto();
        }

        public async Task<QuestionDto> CreateAsync(QuestionDto dto)
        {
            var entity = dto.ToQuestion();
            var created = await _questionRepository.CreateAsync(entity);
            return created.ToQuestionDto();
        }

        public async Task<QuestionDto> UpdateAsync(int id, QuestionDto dto)
        {
            var entity = dto.ToQuestion();
            var updated = await _questionRepository.UpdateAsync(id, entity);
            return updated.ToQuestionDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _questionRepository.DeleteAsync(id);
        }

        public async Task<List<QuestionDto>> GetListAllQuestions()
        {
            var questions = await _questionRepository.GetListAllQuestion();
            return questions.Select(q => q.ToQuestionDto()).ToList();
        }
    }
}
