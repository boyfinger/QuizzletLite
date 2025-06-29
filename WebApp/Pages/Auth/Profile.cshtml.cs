using API.Dtos.User;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WebApp.Pages.Auth
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProfileModel> _logger;

        public ProfileModel(HttpClient httpClient, ILogger<ProfileModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }



        [BindProperty]
        public AvatarUploadDTO AvatarUploadDTO { get; set; } = new AvatarUploadDTO();

        [BindProperty]
        public ChangePasswordDTO ChangePasswordDTO { get; set; } = new ChangePasswordDTO();

        public UserDto? UserDto { get; private set; }

        public IActionResult OnGet()
        {
            UserDto = HttpContext.Session.Get<UserDto>("userSession");
            return UserDto != null ? Page() : RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> OnPostChangeAvatarAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = HttpContext.Session.Get<UserDto>("userSession");
            if (user == null)
                return RedirectToPage("/Auth/Login");

            // Gửi ảnh lên server
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(user.Id.ToString()), "UserId");

            if (AvatarUploadDTO.Avatar?.Length > 0)
            {
                using var stream = new MemoryStream();
                await AvatarUploadDTO.Avatar.CopyToAsync(stream);
                var fileContent = new ByteArrayContent(stream.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(AvatarUploadDTO.Avatar.ContentType);
                form.Add(fileContent, "Avatar", AvatarUploadDTO.Avatar.FileName);
            }
            _logger.LogInformation("form khi gửi: {form}", form.ToString());
            var changeRes = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/change-avatar", form);
            if (!changeRes.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Can't change avatar.");
                return Page();
            }

            var payload = new StringContent(JsonConvert.SerializeObject(new AvatarRequestDTO { UserId = user.Id }), Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/get-user-avatar", payload);
            if (res.IsSuccessStatusCode)
            {
                var dto = JsonConvert.DeserializeObject<AvatarDTO>(await res.Content.ReadAsStringAsync());
                user.Avatar = dto?.Avatar;
                HttpContext.Session.Set("userSession", user);
            }

            TempData["ChangeAvatarSuccess"] = "Avatar updated successfully!";
            return RedirectToPage("/Auth/Profile");
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = HttpContext.Session.Get<UserDto>("userSession");
            if (user == null)
                return RedirectToPage("/Auth/Login");
            return null;
        }
    }
}
