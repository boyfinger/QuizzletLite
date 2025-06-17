using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Pages
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
        public void OnGet()
        {
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
                return Page();
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var userSession = JsonConvert.DeserializeObject<UserDto>(responseData);

            HttpContext.Session.SetString("UserId", userSession.Id.ToString());
            HttpContext.Session.SetString("UserRoleId", userSession.Role.ToString());
            HttpContext.Session.SetString("UserAvatar", userSession.Avatar);
            HttpContext.Session.SetString("UserName", userSession.Username);
            HttpContext.Session.SetString("UserEmail", userSession.Email);

            return RedirectToPage("/Index");
        }
    }
}
