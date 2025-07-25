using API.Dtos.User;
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
            if (Request.Cookies["rememberMe"] == "true")
            {
                loginDTO.RememberMe = true;
                loginDTO.Email = Request.Cookies["email"];
                loginDTO.Password = Request.Cookies["password"];
            }


            var token = HttpContext.Session.GetString("accessToken");

            if (string.IsNullOrEmpty(token))
            {
                return Page();
            }
            return RedirectToPage("/Player/Home");
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
            HttpContext.Session.SetString("accessToken", authResponse.Token);
            if (loginDTO.RememberMe)
            {
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                };
                Response.Cookies.Append("rememberMe", "true", options);
                Response.Cookies.Append("email", loginDTO.Email, options);
                Response.Cookies.Append("password", loginDTO.Password, options);
            }
            else
            {
                Response.Cookies.Delete("rememberMe");
                Response.Cookies.Delete("email");
                Response.Cookies.Delete("password");
            }

            return RedirectToPage("/Player/Home");
        }
    }
}
