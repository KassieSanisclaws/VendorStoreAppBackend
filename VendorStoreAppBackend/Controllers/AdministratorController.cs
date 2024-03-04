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

        // POST [CREATE NEW ADMINISTRATOR]: api/administrators
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
                AdminFirstName = request.AdminFirstName ?? "Admin Not Found",
                AdminLastName = request.AdminLastName ?? "Admin Not Found",
                AdminEmail = request.AdminEmail,
                PhoneNumber = request.PhoneNumber,
                AdminPasswordHash = request.AdminPasswordHash,
                AdminImg = request.AdminImg ?? "Admin Not Found",
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

        // PUT [UPDATE ADMIN]: api/administrators/5 (for example)
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

        // DELETE [ADMIN]: api/administrator/5 (for example)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            await _administratorService.DeleteAdministratorAsync(id);
            return NoContent();
        }


        //TOKEN [REFRESH]: api/admionistrator/refresh-token
        [HttpPost("refresh-token")]
        public ActionResult<RefrshToknResp> RefreshToken([FromBody] RefrshToknResp request)
        {
            var result = _administratorService.RefreshTokenAsync(request.RefreshToken);
            if (result == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }
            return Ok(result);
        }

        //REFRESH TOKEN RESPONSE: api/administrators/refresh-token
        public class RefrshToknResp
        {
            [Required]
            public string? AccessToken { get; set; }
            [Required]
            public string? RefreshToken { get; set; }
        }

        //ADMIN CREATION [REQUEST]: api/administrator/create
        public class AdministratorCreationRequest
        {
            [Required]
            [StringLength(120)]
            public string? AdminFirstName { get; set; }

            [Required]
            [StringLength(120)]
            public string? AdminLastName { get; set; }

            [Required]
            [StringLength(120)]
            [EmailAddress]
            public string? AdminEmail { get; set; }

            [Required]
            [Phone]
            public string? PhoneNumber { get; set; }

            [Required]
            [StringLength(400)]
            public string? AdminPasswordHash { get; set; }

            [Required]
            [StringLength(400)]
            public string? AdminImg { get; set; }

            [StringLength(255)]
            public string? AdminGreetingsIntro { get; set; }
        }


    }
}
