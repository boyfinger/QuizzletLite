namespace API.Helpers
{
    public class QuizQuery
    {
        public string? Name { get; set; } = null;
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }
}
