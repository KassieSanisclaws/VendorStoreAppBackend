using VendorStoreAppBackend.Data;

namespace VendorStoreAppBackend.Services
{
    public class AdministratorService(VendorStoreAppContext context)
    {
        public readonly VendorStoreAppContext _context = context;

        //Get all administrators:
        public async Task<List<Administrators>> GetAllAdministratorsAsync()
        {
            return await _context.Administrators.ToListAsync();
        }
        
}
}
