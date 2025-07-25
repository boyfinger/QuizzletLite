using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WebApp.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult OnGet()
        {
            //if (HttpContext.Session.Get<UserDto>("userSession") == null)
            //{
            //    return Page();
            //}
            //return RedirectToPage("/Home/Home");
            var token = HttpContext.Session.GetString("accessToken");
            if (string.IsNullOrEmpty(token))
            {
                return Page();
            }
            return RedirectToPage("/Player/Home");


        }

        [BindProperty]
        public RegisterDTO registerDTO { get; set; } = new RegisterDTO();

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(registerDTO.Username), "Username");
            form.Add(new StringContent(registerDTO.Email), "Email");
            form.Add(new StringContent(registerDTO.Password), "Password");
            form.Add(new StringContent(registerDTO.ConfirmPassword), "ConfirmPassword");

            if (registerDTO.Avatar != null && registerDTO.Avatar.Length > 0)
            {
                using var stream = new MemoryStream();
                await registerDTO.Avatar.CopyToAsync(stream);
                var fileContent = new ByteArrayContent(stream.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(registerDTO.Avatar.ContentType);
                form.Add(fileContent, "Avatar", registerDTO.Avatar.FileName);
            }

            //var jsonContent = JsonConvert.SerializeObject(registerDTO);
            //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/register", form);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }
            var result = await response.Content.ReadAsStringAsync();
            bool isRegistered = JsonConvert.DeserializeObject<bool>(result);

            if (!isRegistered)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please check your info.");
                return Page();
            }
            TempData["RegisterSuccess"] = "Account created successfully!";
            return RedirectToPage("/Auth/Login");
        }
    }
}
