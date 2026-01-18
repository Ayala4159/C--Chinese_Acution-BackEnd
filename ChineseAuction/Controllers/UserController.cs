using ChineseAuction.Extensions;
using ChineseAuction.Dtos;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userSevice;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userSevice, ILogger<UserController> logger)
        {
            _userSevice = userSevice;
            _logger = logger;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            _logger.LogInformation("Starting to get all users");
            var users = await _userSevice.GetAllUsersAsync();
            _logger.LogInformation("Got all users successfully");
            return Ok(users);
        }


        [Authorize]
        [HttpGet("{id}",Name = "GetUserById")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            if (!User.IsManager() && User.GetUserId() != id)
            {
                return Forbid();
            }
            _logger.LogInformation("Starting to get user by id: {Id}", id);
            var user = await _userSevice.GetUserByIdAsync(id);
            _logger.LogInformation("Got user by id: {Id} successfully", id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] CreateUserDto user)
        {
            if (!User.IsManager() && User.GetUserId() != id)
            {
                return Forbid();
            }
            _logger.LogInformation("Starting to update user with id: {Id}", id);
            try
            {
                var updatedUser = await _userSevice.UpdateUserAsync(id, user);
                _logger.LogInformation("Updated user with id: {Id} successfully", id);
                return updatedUser == null ? NotFound() : Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            if (User.IsManager() || User.GetUserId() != id)
            {
                return Forbid();
            }
            _logger.LogInformation("Starting to delete user with id: {Id}", id);
            try
            {
                await _userSevice.DeleteUserAsync(id);
                _logger.LogInformation("Deleted user with id: {Id} successfully", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest(ex.Message);
            }
        }


        // Login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] loginRequestDto userDto)
        {
            _logger.LogInformation("Starting user authentication for email: {Email}", userDto.Email);
            var authResponse = await _userSevice.AuthenticateAsync(userDto);
            _logger.LogInformation("Completed user authentication for email: {Email}", userDto.Email);
            return authResponse == null ? Unauthorized() : Ok(authResponse);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] CreateUserDto userDto)
        {
            _logger.LogInformation("Starting user registration for email: {Email}", userDto.Email);
            try
            {
                var newUser = await _userSevice.AddUserAsync(userDto);
                if (newUser == null)
                {
                    return BadRequest("User could not be create");
                }
                _logger.LogInformation("User registration successful for email: {Email}", userDto.Email);
                return CreatedAtRoute("GetUserById", new { id = newUser.Id },newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registeration");
                return BadRequest(ex.Message);
            }
        }
    }
}