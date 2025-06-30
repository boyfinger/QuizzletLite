namespace API.Helpers
{
    public class UserQuery
    {
        public string? Keyword { get; set; }  // Từ khóa tìm kiếm dùng chung
        public Role? Role { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
