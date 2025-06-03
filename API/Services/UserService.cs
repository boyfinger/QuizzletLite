﻿using API.Dtos.User;
using API.Helpers;
using API.Mappers;
using API.Repositories;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetUsers(UserQuery query)
        {
            var users = await _userRepository.GetUsers(query);
            return users.Select(u => u.ToUserDto()).ToList();
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user?.ToUserDto();
        }

        public async Task AddUser(UserDto userDto)
        {
            var user = userDto.ToUser();
            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            if (!await _userRepository.UserExists(userDto.Id))
            {
                throw new Exception($"User with ID {userDto.Id} does not exist.");
            }
            await _userRepository.UpdateUser(userDto);
        }

        public async Task<bool> DeleteUser(int id)
        {
            if (!await _userRepository.UserExists(id))
            {
                return false;
            }

            return await _userRepository.DeleteUser(id);
        }

        public async Task<bool> UserExists(int id)
        {
            return await _userRepository.UserExists(id);
        }
    }
}
