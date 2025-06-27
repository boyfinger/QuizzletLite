using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var jsonContent = JsonConvert.SerializeObject(registerDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/register", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }
            return null;
        }
    }
}
