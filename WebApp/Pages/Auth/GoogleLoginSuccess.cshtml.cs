using API.Dtos.User;
using API.Models.Enums;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Pages.Auth
{
    public class GoogleLoginSuccessModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public GoogleLoginSuccessModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public UserDto? UserDto { get; set; }
        public string? Token { get; set; }

        public async Task<IActionResult> OnGet(string token, int id, string username, string email, Role role)
        {
            string? avatar = null;

            try
            {
                var payload = new StringContent(JsonConvert.SerializeObject(new AvatarRequestDTO { UserId = id }), Encoding.UTF8, "application/json");
                var res = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/get-user-avatar", payload);

                if (res.IsSuccessStatusCode)
                {
                    var dto = JsonConvert.DeserializeObject<AvatarDTO>(await res.Content.ReadAsStringAsync());
                    avatar = dto?.Avatar;
                }
            }
            catch { }

            UserDto = new UserDto
            {
                Id = id,
                Username = username,
                Email = email,
                Role = role,
                Avatar = avatar
            };

            Token = token;
            HttpContext.Session.Set("userSession", UserDto);
            HttpContext.Session.SetString("accessToken", token);

            return RedirectToPage("/Index");
        }
    }
}
