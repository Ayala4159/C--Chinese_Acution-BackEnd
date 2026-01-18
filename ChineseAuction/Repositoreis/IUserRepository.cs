using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateUserAsync(User user);
        Task<bool> UserEmailExistsAsync(string email, int id);
    }
}