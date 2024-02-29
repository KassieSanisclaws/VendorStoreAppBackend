using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorStoreAppBackend.Data;
using VendorStoreAppBackend.Entities_Models;

namespace VendorStoreAppBackend.Services
{
    public class UserService(VendorStoreAppContext context)
    {
        private readonly VendorStoreAppContext _context = context;

        // Get all users:
        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Get user by id:
        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Create new user:
        public async Task<Users> CreateUserAsync(Users user, string password)
        {
            // Hash the password before storing it
            user.UsersPasswordHash = BCrypt.Net.BCrypt.HashPassword(user.UsersPasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Update user by id:
        public async Task<Users> UpdateUserAsync(int id, Users user)
        {
            var existingUser = await _context.Users.FindAsync(id) ?? throw new ArgumentException("User not found");
            existingUser.UsersFirstName = user.UsersFirstName;
            existingUser.UsersLastName = user.UsersLastName;
            // Update other properties as needed...

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Delete user by id:
        public async Task<Users> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new ArgumentException("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Authenticate user and generate JWT token
        public async Task<string?> AuthenticateAndGetTokenAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UsersEmail == email);

            // Check if the user exists and the password is correct
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.UsersPasswordHash))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("YourSecretKey"); // Replace with secret key
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]    
                    {
                        new(ClaimTypes.Name, user.UsersId.ToString())
                        // You can add more claims as needed, such as roles, etc.
                    }),
                    Expires = DateTime.UtcNow.AddHours(4), // Token expiration time
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature) //Hashing Algorithm strength 
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null; // Authentication failed
        }
    }
}
