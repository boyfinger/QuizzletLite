using API.Dtos.User;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetUsers(UserQuery query);
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);
        Task AddUser(User user);
        Task UpdateUser(UserDto dto);
        Task<bool> DeleteUser(int id);
        Task<bool> UserExists(int id);
        Task<string?> GetAvatarByUserId(int userId);
        IQueryable<User> GetUsersQueryable();
        
    }
}
