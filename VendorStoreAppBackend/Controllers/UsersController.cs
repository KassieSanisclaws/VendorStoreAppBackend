﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VendorStoreAppBackend.Entities_Models;
using VendorStoreAppBackend.Services;

namespace VendorStoreAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
        
    {
        private readonly UserService _userService = userService;

        // GET [ALL]: api/Users:
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = request.User;
            var password = request.Password;

            if (user == null || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid user or password");
            }

            await _userService.CreateUserAsync(user, password);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UsersId }, user);
        }

        [HttpPut("{id}")]
        [Authorize] // Only Authenticated Users can access this endpoint
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.UsersId)
            {
                return BadRequest("User Update UnSuccessful!");
            }
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can access this endpoint
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        //TOKEN [REFRESH]: api/users/refresh-token
        [HttpPost("refresh-token")]
        public ActionResult<TokenResponse> RefreshToken([FromBody] TokenResponse request)
        {
            var result = _userService.RefreshAccessTokenAsync(request.RefreshToken ?? "Invalid Token");
            if (result == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }
            return Ok(result);
        }

        //REFRESH TOKEN RESPONSE: api/users/refresh-token
        public class TokenResponse
        {
            [Required]
            public string? AccessToken { get; set; }
            [Required]
            public string? RefreshToken { get; set; }
        }

       //USER CREATION [REQUEST]: api/users/create
        public class CreateUserRequest
        {
            public Users? User { get; set; }
            public string? Password { get; set; }
            public string? UserName { get; set; }
        }

    }
}
