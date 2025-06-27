using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class GoogleLoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("https://localhost:7245/api/Authentication/login-google");
        }
    }
}
