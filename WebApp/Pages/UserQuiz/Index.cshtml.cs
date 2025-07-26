using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.UserQuiz
{
    public class IndexModel : PageModel
    {
        public string? AccessToken { get; set; }

        public void OnGet()
        {
            AccessToken = HttpContext.Session.GetString("accessToken");
        }
    }
}
