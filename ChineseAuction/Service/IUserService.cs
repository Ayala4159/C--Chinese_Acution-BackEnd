using ChineseAuction.Dtos;
using ChineseAuction.Models;

namespace ChineseAuction.Service
{
    public interface IUserService
    {
        Task<GetUserDto> AddUserAsync(CreateUserDto user);
        Task<loginResponseDto?> AuthenticateAsync(loginRequestDto loginUser);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<GetUserDto?> UpdateUserAsync(int id, CreateUserDto user);
    }
}