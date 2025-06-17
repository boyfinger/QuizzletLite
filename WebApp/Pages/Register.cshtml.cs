using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public RegisterDTO registerDTO { get; set; } = new RegisterDTO();
    }
}
