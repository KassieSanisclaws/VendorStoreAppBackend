using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VendorStoreAppBackend.Entities_Models;
using VendorStoreAppBackend.Services;

namespace VendorStoreAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController(AdministratorService administratorService) : ControllerBase
    {
        private readonly AdministratorService _administratorService = administratorService;

        // GET [ALL]: api/administrators
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Administrators>>> GetAllAdministrators()
        {
            return await _administratorService.GetAllAdministratorsAsync();
        }

        // GET [ByID]: api/admin/5 (for example)
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAdministratorByID(int id)
        {
            var administrator = await _administratorService.GetAdministratorByIdAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return Ok(administrator);
        }

        // POST [CREATE NEW ADMINISTRATOR]: api/vendors
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<ActionResult> CreateNewAdministrator([FromBody] AdministratorCreationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administrator = new Administrators
            {
                AdminName = request.AdminName,
                AdminEmail = request.AdminEmail,
                AdminNumber = request.AdminNumber,
                AdminAddress = request.AdminAddress,
                AdminPasswordHash = request.AdminPasswordHash,
                AdminImg = request.AdminImg
            };

            try
            {
                await _administratorService.CreateAdministratorAsync(administrator);
                return CreatedAtAction(nameof(GetAdministratorByID), new { id = administrator.AdminId }, administrator);
            }
            catch (Exception ex)
            {
                // Log the exception message & return a 500 Internal Server Error handling the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // PUT [UPDATE VENDOR]: api/administrators/5 (for example)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAdminsitratorID(int id, Administrators administrator)
        {
            if (id != administrator.AdminId)
            {
                return BadRequest("Administrator Update Unsuccessful!");
            }
            await _administratorService.UpdateAdministratorAsync(id, administrator);
            return NoContent();
        }

        // DELETE [VENDOR]: api/administrator/5 (for example)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            await _administratorService.DeleteAdminsitratorAsync(id);
            return NoContent();
        }


        //TOKEN [REFRESH]: api/vendors/refresh-token
        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefrshToknResp>> RefreshToken([FromBody] RefrshToknResp request)
        {
            var result = _administratorService.RefreshTokenAsync(request.RefreshToken);
            if (result == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }
            return Ok(result);
        }

        //REFRESH TOKEN RESPONSE: api/vendors/refresh-token
        public class RefrshToknResp
        {
            [Required]
            public string? AccessToken { get; set; }
            [Required]
            public string? RefreshToken { get; set; }
        }

        //VENDOR CREATION [REQUEST]: api/vendors/create
        public class AdministratorCreationRequest
        {
            [Required]
            [StringLength(120)]
            public string? VendorName { get; set; }

            [Required]
            [StringLength(120)]
            [EmailAddress]
            public string? VendorEmail { get; set; }

            [Required]
            [StringLength(10)]
            public string? VendorNumber { get; set; }

            [Required]
            [StringLength(120)]
            public string? VendorAddress { get; set; }

            [Required]
            [StringLength(120)]
            public string? VendorBussRegID { get; set; }

            [Required]
            [StringLength(120)]
            public string? VendorBussLicense { get; set; }

            [Required]
            [StringLength(400)]
            public string? VendorPasswordHash { get; set; }

            [Required]
            [StringLength(400)]
            public string? VendorImg { get; set; }

            [StringLength(255)]
            public string? VendorGreetingsIntro { get; set; }
        }


    }
}
