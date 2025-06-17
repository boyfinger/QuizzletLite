using API.Models.Enums;

namespace API.Helpers
{
    public class UserQuery
    {
        public string? Username { get; set; } = null;
        public string? Email { get; set; } = null;
        public Role? Role { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }
}
