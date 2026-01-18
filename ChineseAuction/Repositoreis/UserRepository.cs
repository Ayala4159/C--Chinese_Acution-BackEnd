using ChineseAuction.Data;
using ChineseAuction.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ChineseAuction.Repositoreis
{
    public class UserRepository : IUserRepository
    {
        private readonly ChinesActionDbContext _context;
        public UserRepository(ChinesActionDbContext context)
        {
            _context = context;
        }

        //get all users -manager
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // get user by id -manager
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(u=>u.Purchase).FirstOrDefaultAsync(p=>p.Id==id);
        }

        // add user -everyOne
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // update user -admin
        public async Task<User?> UpdateUserAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existing;
        }

        // delete user -admin
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // get user by email - manager
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // uniq email
        public async Task<bool> UserEmailExistsAsync(string email, int id)
        {
            return await _context.Users.AnyAsync(d => d.Email == email && d.Id != id);
        }

        // filter user by name, email, gift name -manager
        public async Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.First_name.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(d => d.Email.Contains(email));

            return await query.ToListAsync();

        }

        // get user by email
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}