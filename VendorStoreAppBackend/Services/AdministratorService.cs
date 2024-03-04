using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorStoreAppBackend.Data;
using VendorStoreAppBackend.Entities_Models;

namespace VendorStoreAppBackend.Services
{
    public class AdministratorService(VendorStoreAppContext context)
    {
        private readonly VendorStoreAppContext _context = context;

        //Get all administrators:
        public async Task<List<Administrators>> GetAllAdministratorsAsync()
        {
            return await _context.Administrators.ToListAsync();
        }
        
        //Get administrator by id:
        public async Task<Administrators?> GetAdministratorByIdAsync(int id)
        {
            return await _context.Administrators.FindAsync(id);
        }

        // Update Administrator by id:
        public async Task<Administrators> UpdateAdministratorAsync(int id, Administrators administrator)
        {
            var existingAdmin = await _context.Administrators.FindAsync(id) ?? throw new ArgumentException("Administrator not found");
            existingAdmin.AdminFirstName = administrator.AdminFirstName;
            existingAdmin.AdminLastName = administrator.AdminLastName;
            existingAdmin.AdminEmail = administrator.AdminEmail;
            existingAdmin.AdminId = administrator.AdminId;
            //existingAdmin.
            // Update other properties as needed...

            await _context.SaveChangesAsync();
            return existingAdmin;
        }

        // Delete vendor by id:
        public async Task<Administrators> DeleteAdministratorAsync(int id)
        {
            var administrator = await _context.Administrators.FindAsync(id) ?? throw new ArgumentException("Administrator not found");
            _context.Administrators.Remove(administrator);
            await _context.SaveChangesAsync();
            return administrator;
        }

        // Create new administrator:
        public async Task<Administrators> CreateAdministratorAsync(Administrators administrator)
        {
            _context.Administrators.Add(administrator);
            await _context.SaveChangesAsync();
            return administrator;
        }

        //Authenticate administrator and generate JWT token
        public async Task<string?> AuthenticateAdministratorAsync(string adminName, string adminPassword)
        {
            var administrator = await _context.Administrators.SingleOrDefaultAsync(x => x.AdminFirstName == adminName);
            if (administrator == null || !BCrypt.Net.BCrypt.Verify(adminPassword, administrator.AdminPasswordHash))
            {
                return null;
            }
            return GenerateJwtToken(administrator);
        }

        //REFRESH TOKEN
        public async Task<string?> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var administrator = await _context.Administrators.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (administrator == null)
            {
                return null;
            }

            return GenerateJwtToken(administrator);
        }

        //Generate JWT token
        private static string GenerateJwtToken(Administrators administrators)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("suPerRsecretKeyAsExamplE579433");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 //Performs check first for null value before assigning
                 {
                     new(ClaimTypes.Name, value: administrators.AdminFirstName ?? "Vendor Not Found"),
                     new(ClaimTypes.Role, "Admin"),
                     new(JwtRegisteredClaimNames.Email, administrators.AdminEmail ?? "Vendor Email Not Found"),
                     new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ?? "Jti Not Found"),
                     new(JwtRegisteredClaimNames.Sub, administrators.AdminEmail ?? "Vendor Email Not Found")
                 }),
                Expires = DateTime.UtcNow.AddHours(4), //Hours before expriation of token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Hash Algorithm strength
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

    }
}
