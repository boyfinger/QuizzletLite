using API.Dtos.User;
using API.Models.Enums;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class GoogleLoginSuccessModel : PageModel
    {
        public UserDto? UserDto { get; set; }
        public string? Token { get; set; }

        public IActionResult OnGet(string token, int id, string username, string email, string avatar, Role role)
        {
            UserDto = new UserDto
            {
                Id = id,
                Username = username,
                Email = email,
                Avatar = avatar,
                Role = role
            };

            Token = token;

            HttpContext.Session.Set<UserDto>("userSession", UserDto);

            HttpContext.Session.SetString("accessToken", Token); ;

            return RedirectToPage("/Index");
        }
    }
}
