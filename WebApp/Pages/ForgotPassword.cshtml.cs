using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ForgotPasswordModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public ForgotPasswordDTO forgotPasswordDTO { get; set; } = new ForgotPasswordDTO();
        public void OnGet()
        {
        }
    }
}
