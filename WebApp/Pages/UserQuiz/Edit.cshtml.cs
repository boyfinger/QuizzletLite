using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.UserQuiz
{
    public class EditModel : PageModel
    {
        public string? AccessToken { get; set; }

        public void OnGet()
        {
            AccessToken = HttpContext.Session.GetString("accessToken");
        }
    }
}
