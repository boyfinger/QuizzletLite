﻿using API.Dtos.User;
using API.Helpers;

namespace API.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers(UserQuery query);
        Task<UserDto?> GetUserById(int id);
        Task AddUser(UserDto userDto);
        Task UpdateUser(UserDto userDto);
        Task<bool> DeleteUser(int id);
        Task<bool> UserExists(int id);
    }
}
