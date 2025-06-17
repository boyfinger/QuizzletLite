using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string Username { get; set; } = "Guest";
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("UserName") ?? "Guest";
        }
    }
}
