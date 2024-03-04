using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VendorStoreAppBackend.Entities_Models;
using VendorStoreAppBackend.Services;

namespace VendorStoreAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController(VendorService vendorService) : ControllerBase
    {
        private readonly VendorService _vendorService = vendorService;

        // GET [ALL]: api/vendors
        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public async Task<ActionResult<IEnumerable<Vendors>>> GetVendors()
        {
            return await _vendorService.GetAllVendorsAsync();
        }

        // GET [ByID]: api/vendor/5 (for example)
        [HttpGet("{id}")]
        public async Task<ActionResult> GetVendorByID(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return Ok(vendor);
        }

        // POST [CREATE NEW VENDOR]: api/vendors
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<ActionResult> CreateNewVendor([FromBody] VendorCreationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendor = new Vendors
            {
                VendorName = request.VendorName,
                VendorEmail = request.VendorEmail,
                VendorNumber = request.VendorNumber,
                Address = request.VendorAddress,
                VendorBussRegID = request.VendorBussRegID,
                VendorBussLicense = request.VendorBussLicense,
                VendorPasswordHash = request.VendorPasswordHash,
                VendorImg = request.VendorImg,
                VendorGreetingsIntro = request.VendorGreetingsIntro
            };

           try {
               await _vendorService.CreateVendorAsync(vendor);
              return CreatedAtAction(nameof(GetVendorByID), new { id = vendor.VendorId }, vendor);
            }
           catch(Exception ex)
            {
             // Log the exception message & return a 500 Internal Server Error handling the exception
              return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        // PUT [UPDATE VENDOR]: api/vendors/5 (for example)
         [HttpPut("{id}")]
         [Authorize(Roles = "Vendor")]
         public async Task<ActionResult> UpdateVendorID(int id, Vendors vendor)
           {
            if (id != vendor.VendorId)
            {
                return BadRequest("Vendor Update Unsuccessful!");
            }
            await _vendorService.UpdateVendorAsync(id, vendor);
            return NoContent();
           }

        // DELETE [VENDOR]: api/vendors/5 (for example)
         [HttpDelete("{id}")]
         [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
         public async Task<ActionResult> DeleteVendor(int id)
           {
            await _vendorService.DeleteVendorAsync(id);
            return NoContent();
           }


        //TOKEN [REFRESH]: api/vendors/refresh-token
        [HttpPost("refresh-token")]
        public ActionResult<RefrshToknResp> RefreshToken([FromBody] RefrshToknResp request)
        {
            var result = _vendorService.RefreshTokenAsync(request.RefreshToken);
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
        public class VendorCreationRequest
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
