using Microsoft.EntityFrameworkCore;
using VendorStoreAppBackend.Data;
using VendorStoreAppBackend.Entities_Models;

namespace VendorStoreAppBackend.Services
{
    public class UserService(VendorStoreAppContext context)
    {
        private readonly VendorStoreAppContext _context = context;

        // Get all users
        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Get user by id
        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Create new user
        public async Task<Users> CreateUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Update user by id
        public async Task<Users> UpdateUserAsync(int id, Users user)
        {
            var existingUser = await _context.Users.FindAsync(id) ?? throw new ArgumentException("User not found");
            existingUser.UsersFirstName = user.UsersFirstName;
            existingUser.UsersLastName = user.UsersLastName;
            // Update other properties as needed...

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Delete user by id
        public async Task<Users> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
