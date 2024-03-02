using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorStoreAppBackend.Data;
using VendorStoreAppBackend.Entities_Models;

namespace VendorStoreAppBackend.Services
{
    public class VendorService(VendorStoreAppContext context)
    {
         private readonly VendorStoreAppContext _context = context;

        // Get all vendors:
        public async Task<List<Vendors>> GetAllVendorsAsync()
            {
                return await _context.Vendors.ToListAsync();
            }

            //Get Vendor by id:
            public async Task<Vendors?> GetVendorByIdAsync(int id)
            {
                return await _context.Vendors.FindAsync(id);
            }

            // Update vendor by id:
            public async Task<Vendors> UpdateVendorAsync(int id, Vendors vendor)
            {
                var existingVendor = await _context.Vendors.FindAsync(id) ?? throw new ArgumentException("Vendor not found");
                existingVendor.VendorName = vendor.VendorName;
                existingVendor.VendorEmail = vendor.VendorEmail;
                existingVendor.VendorId = vendor.VendorId;
                existingVendor.VendorBussRegID = vendor.VendorBussRegID;
                //existingVendor.
                // Update other properties as needed...

                await _context.SaveChangesAsync();
                return existingVendor;
            }

            // Delete vendor by id:
            public async Task<Vendors> DeleteVendorAsync(int id)
            {
                var vendor = await _context.Vendors.FindAsync(id) ?? throw new ArgumentException("Vendor not found");

                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
                return vendor;
            }

            // Create new vendor:
            public async Task<Vendors> CreateVendorAsync(Vendors vendor)
            {
                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();
                return vendor;
            }

            //Authenticate vendor and generate JWT token
            public async Task<string?> AuthenticateVendorAsync(string vendorName, string vendorPassword)
            {
                var vendor = await _context.Vendors.SingleOrDefaultAsync(x => x.VendorName == vendorName);

                if (vendor == null || !BCrypt.Net.BCrypt.Verify(vendorPassword, vendor.VendorPasswordHash))
                {
                    return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("suPerRsecretKeyAsExamplE579433");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                     //Performs check first for null value before assigning
                     {
                     new(ClaimTypes.Name, value: vendor.VendorName ?? "Vendor Not Found"),
                     new(ClaimTypes.Role, "Vendor")
                     }),
                    Expires = DateTime.UtcNow.AddHours(4), //Hours before expriation of token
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Hash Algorithm strength
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

        }
}
