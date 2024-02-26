using Microsoft.EntityFrameworkCore;
using VendorStoreAppBackend.Entities_Models;

namespace VendorStoreAppBackend.Data
{
    public class VendorStoreAppContext(DbContextOptions<VendorStoreAppContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<User_Roles> User_Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Permissions_Resource> Permissions_Resource { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

    }
}
