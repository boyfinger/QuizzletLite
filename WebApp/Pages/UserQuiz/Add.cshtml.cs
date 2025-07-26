using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.UserQuiz
{
    public class AddModel : PageModel
    {
        public string? AccessToken { get; set; }

        public void OnGet()
        {
            AccessToken = HttpContext.Session.GetString("accessToken");
        }
    }
}
 