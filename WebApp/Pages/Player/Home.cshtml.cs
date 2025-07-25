using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;

        public HomeModel(ILogger<HomeModel> logger)
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
