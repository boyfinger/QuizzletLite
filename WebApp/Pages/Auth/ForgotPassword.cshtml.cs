using API.Dtos.User;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ForgotPasswordModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public ForgotPasswordDTO ForgotPasswordDTO { get; set; } = new ForgotPasswordDTO();
        public IActionResult OnGet()
        {
            if (HttpContext.Session.Get<UserDto>("userSession") == null)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostSendResetLinkAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var jsonContent = JsonConvert.SerializeObject(ForgotPasswordDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7245/api/authentication/send-reset-otp", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["ResetError"] = "Failed to send OTP. Please try again.";
                ModelState.AddModelError(string.Empty, "Failed to send OTP. Please check your input.");
                return Page();
            }

            TempData["ResetInfo"] = "An OTP has been sent to your email.";
            return RedirectToPage("/Auth/ResetWithOtp");
        }
    }
}
