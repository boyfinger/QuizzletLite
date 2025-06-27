using API.Models;
using System.Security.Claims;

namespace API.Services
{
    public interface IGoogleAuthService
    {
        Task<User> HandleGoogleLoginAsync(ClaimsPrincipal principal);
    }
}
