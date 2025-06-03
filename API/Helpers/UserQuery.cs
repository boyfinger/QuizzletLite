namespace API.Helpers
{
    public class UserQuery
    {
        public string? Username { get; set; } = null;
        public string? Email { get; set; } = null;
        public int? RoleId { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }
}
