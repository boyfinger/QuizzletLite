﻿using API.Dtos.User;
using API.Helpers;
using API.Models;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers(UserQuery query);
        Task<User?> GetUserById(int id);
        Task AddUser(User user);
        Task UpdateUser(UserDto dto);
        Task<bool> DeleteUser(int id);
        Task<bool> UserExists(int id);
    }
}
