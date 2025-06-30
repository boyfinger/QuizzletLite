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

        public UserDto? UserDto { get; private set; }

        public IActionResult OnGet()
        {
            UserDto = HttpContext.Session.Get<UserDto>("userSession");
            return UserDto != null ? Page() : RedirectToPage("/Auth/Login");
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

            var user = HttpContext.Session.Get<UserDto>("userSession");
            if (user == null)
            {
                TempData["ChangeAvatarFailed"] = "Your session has expired. Please log in again.";
                return RedirectToPage("/Auth/Login");
            }


            // Gửi ảnh lên server
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(user.Id.ToString()), "UserId");

            if (avatarUploadDTO.Avatar?.Length > 0)
            {
                Console.WriteLine(avatarUploadDTO.Avatar);
                using var stream = new MemoryStream();
                await avatarUploadDTO.Avatar.CopyToAsync(stream);
                var fileContent = new ByteArrayContent(stream.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(avatarUploadDTO.Avatar.ContentType);
                form.Add(fileContent, "Avatar", avatarUploadDTO.Avatar.FileName);
            }

            var token = HttpContext.Session.GetString("accessToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
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

        public async Task<IActionResult> OnPostChangePasswordAsync([FromForm] ChangePasswordDTO changePasswordDTO)
        {
            if (!TryValidateModel(changePasswordDTO))
            {
                TempData["ChangePasswordFailed"] = "Please correct the errors in the password form.";
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"ChangePassword ModelState Error: {error.ErrorMessage}");
                    }
                }
                return RedirectToPage("/Auth/Profile");
            }

            var user = HttpContext.Session.Get<UserDto>("userSession");
            if (user == null)
                return RedirectToPage("/Auth/Login");
            changePasswordDTO.UserId = user.Id;
            var jsonContent = JsonConvert.SerializeObject(changePasswordDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("accessToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/change-password", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Can't change password.");
                return Page();
            }
            var result = await response.Content.ReadAsStringAsync();
            bool isChanged = JsonConvert.DeserializeObject<bool>(result);
            if (!isChanged)
            {
                ModelState.AddModelError(string.Empty, "Change password failed.");
                return Page();
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
            var user = HttpContext.Session.Get<UserDto>("userSession");
            if (user == null)
            {
                TempData["UpdateProfileError"] = "Your session has expired. Please log in again.";
                return RedirectToPage("/Auth/Login");
            }
            updateProfileDTO.UserId = user.Id;
            var jsonContent = JsonConvert.SerializeObject(updateProfileDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("accessToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsync("https://localhost:7245/api/Authentication/update-profile", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Can't change password.");
                return Page();
            }
            var responseData = await response.Content.ReadAsStringAsync();
            var isChanged = JsonConvert.DeserializeObject<bool>(responseData);
            if (isChanged)
            {

                var getUserResponse = await _httpClient.GetAsync($"https://localhost:7245/api/User/{user.Id}");

                var userJson = await getUserResponse.Content.ReadAsStringAsync();
                var updatedUserDto = JsonConvert.DeserializeObject<UserDto>(userJson);

                if (updatedUserDto != null)
                {
                    HttpContext?.Session.Set("userSession", updatedUserDto);
                    TempData["UpdateProfileSuccess"] = "Profile updated successfully!";
                }
                else
                {
                    TempData["UpdateProfileError"] = "Profile updated, but failed to retrieve updated user data.";
                }
                TempData["UpdateProfileSuccess"] = "Profile updated successfully!";
                return RedirectToPage("/Auth/Profile");
            }
            else
            {
                TempData["UpdateProfileError"] = "Profile updated, but failed to retrieve updated user data.";
                return RedirectToPage("/Auth/Profile");
            }
        }
    }
}
