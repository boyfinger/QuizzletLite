using API.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        public UserDto? UserDto { get; private set; }

        public IActionResult OnGet()
        {
            var token = HttpContext.Session.GetString("accessToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Player/Home");
            }
            return Page();
            //UserDto = HttpContext.Session.Get<UserDto>("userSession");
            //return UserDto != null ? Page() : RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> OnPostChangeAvatarAsync([FromForm] AvatarUploadDTO avatarUploadDTO)
        {
            if (!TryValidateModel(avatarUploadDTO))
            {
                TempData["ChangeAvatarFailed"] = "Please select a valid avatar file or check for errors.";
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"Avatar ModelState Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var token = HttpContext.Session.GetString("accessToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["ChangeAvatarFailed"] = "Session expired. Please login.";
                return RedirectToPage("/Auth/Login");
            }

            // 🔐 Decode token để lấy UserId
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ChangeAvatarFailed"] = "Invalid token. Please login.";
                return RedirectToPage("/Auth/Login");
            }

            // 🖼 Gửi ảnh lên server
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(userId), "UserId");

            if (avatarUploadDTO.Avatar?.Length > 0)
            {
                using var stream = new MemoryStream();
                await avatarUploadDTO.Avatar.CopyToAsync(stream);
                var fileContent = new ByteArrayContent(stream.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(avatarUploadDTO.Avatar.ContentType);
                form.Add(fileContent, "Avatar", avatarUploadDTO.Avatar.FileName);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var changeRes = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/change-avatar", form);

            if (!changeRes.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Can't change avatar.");
                return Page();
            }

            TempData["ChangeAvatarSuccess"] = "Avatar updated successfully!";
            return RedirectToPage("/Auth/Profile");
        }

        public async Task<IActionResult> OnPostChangePasswordAsync([FromForm] ChangePasswordDTO dto)
        {
            if (!TryValidateModel(dto))
            {
                TempData["ChangePasswordFailed"] = "Please correct the errors in the password form.";
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        _logger.LogError($"ChangePassword validation error: {error.ErrorMessage}");
                    }
                }
                return RedirectToPage("/Auth/Profile");
            }

            var token = HttpContext.Session.GetString("accessToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["ChangePasswordFailed"] = "Session expired. Please log in again.";
                return RedirectToPage("/Auth/Login");
            }

            // 🔍 Decode JWT để lấy userId
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int uid))
            {
                TempData["ChangePasswordFailed"] = "User info could not be extracted.";
                return RedirectToPage("/Auth/Login");
            }

            dto.UserId = uid;

            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync("https://localhost:7245/api/authentication/change-password", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ChangePasswordFailed"] = "Can't change password.";
                return RedirectToPage("/Auth/Profile");
            }

            var result = await response.Content.ReadAsStringAsync();
            if (!JsonConvert.DeserializeObject<bool>(result))
            {
                TempData["ChangePasswordFailed"] = "Change password failed.";
                return RedirectToPage("/Auth/Profile");
            }

            TempData["ChangePasswordSuccess"] = "Password changed successfully!";
            return RedirectToPage("/Auth/Profile");
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync([FromForm] UpdateProfileDTO updateProfileDTO)
        {
            if (!TryValidateModel(updateProfileDTO))
            {
                TempData["UpdateProfileError"] = "Please correct the errors in the form.";
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"UpdateProfile ModelState Error: {error.ErrorMessage}");
                    }
                }
                return RedirectToPage("/Auth/Profile");
            }

            var token = HttpContext.Session.GetString("accessToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["UpdateProfileError"] = "Your session has expired. Please log in again.";
                return RedirectToPage("/Auth/Login");
            }

            // 🔍 Decode JWT để lấy UserId
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["UpdateProfileError"] = "Unable to extract user info from token.";
                return RedirectToPage("/Auth/Login");
            }

            // Gán vào DTO mà không cần session
            updateProfileDTO.UserId = int.Parse(userId);

            // Gửi lên backend
            var jsonContent = JsonConvert.SerializeObject(updateProfileDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/update-profile", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Can't update profile.");
                return Page();
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var isChanged = JsonConvert.DeserializeObject<bool>(responseData);

            TempData[isChanged ? "UpdateProfileSuccess" : "UpdateProfileError"] =
                isChanged ? "Profile updated successfully!" : "Profile update failed.";

            return RedirectToPage("/Auth/Profile");
        }
    }
}
