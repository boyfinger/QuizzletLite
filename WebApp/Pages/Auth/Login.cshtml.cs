using API.Dtos.User;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Pages.Auth
{
    public class LoginModel : PageModel
    {

        private readonly HttpClient _httpClient;
        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public LoginDTO loginDTO { get; set; } = new LoginDTO();
        public IActionResult OnGet()
        {
            if (HttpContext.Session.Get<UserDto>("userSession") == null)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var jsonContent = JsonConvert.SerializeObject(loginDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/login", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                TempData["LoginFailed"] = "Invalid email or password. Please try again.";
                return Page();
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponseDto>(responseData);

            HttpContext.Session.Set("userSession", authResponse.UserDto);

            HttpContext.Session.SetString("accessToken", authResponse.Token);

            return RedirectToPage("/Index");
        }
    }
}
