using API.Dtos.User;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Pages.Auth
{
    public class ResetWithOtpModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ResetWithOtpModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public ResetWithOtpInputDto ResetWithOtpInputDto { get; set; } = new ResetWithOtpInputDto();
        public IActionResult OnGet()
        {
            if (HttpContext.Session.Get<UserDto>("userSession") == null)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostResetPasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var jsonContent = JsonConvert.SerializeObject(ResetWithOtpInputDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7245/api/authentication/verify-otp", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["ResetFailed"] = "Reset password failed !";
                ModelState.AddModelError(string.Empty, "Failed to reset password. Please check your input.");
                return Page();
            }
            TempData["ResetSuccess"] = "Reset password successfully !";
            return RedirectToPage("/Auth/Login");
        }
    }
}
