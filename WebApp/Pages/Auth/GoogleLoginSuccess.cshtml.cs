using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Auth
{
    public class GoogleLoginSuccessModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public GoogleLoginSuccessModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public string? Token { get; set; }

        public async Task<IActionResult> OnGet(string token)
        {
            Token = token;
            HttpContext.Session.SetString("accessToken", token);

            return RedirectToPage("/Player/Home");
        }
    }
}
