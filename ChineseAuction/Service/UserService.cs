using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Repositoreis;
using ChineseAuction.Services;
using Microsoft.Extensions.Configuration;

namespace ChineseAuction.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _tokenService = tokenService;
            _logger = logger;

        }
        // Service methods for handling business logic related to Donor entity


        //get all users -manager
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<User>>(users);
        }

        // get user by id -manager
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // add user -everyOne
        public async Task<GetUserDto> AddUserAsync(CreateUserDto user)
        {
            if (await UserEmailExistsAsync(user.Email, -1))
            {
                _logger.LogWarning("Attempt to register with existing email: {Email}", user.Email);
                throw new Exception("Email already exists");
            }
            user.Password = HashPassword(user.Password);
            var newUser = _mapper.Map<User>(user);
            await _userRepository.AddUserAsync(newUser);
            return _mapper.Map<GetUserDto>(newUser);
        }
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        // update user -admin
        public async Task<GetUserDto?> UpdateUserAsync(int id, CreateUserDto user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("Attempt to update non-existing user with ID: {UserId}", id);
                return null;
            }
            if (user.Email != existingUser.Email && user.Email != null)
            {
                if (await UserEmailExistsAsync(user.Email, id))
                    _logger.LogWarning("Attempt to update user ID {UserId} with existing email: {Email}", id, user.Email);
                throw new Exception("User with the same email already exists.");
            }
            _mapper.Map(user, existingUser);
            if (user.Password != null)
            {
                existingUser.Password = HashPassword(user.Password);
            }
                existingUser.Id = id;
                var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
                if (updatedUser == null)
                {
                    _logger.LogError("Failed to update user with ID: {UserId}", id);
                    return null;
                }
            
                return _mapper.Map<GetUserDto>(updatedUser);
        }


        // delete user -admin
        public async Task<bool> DeleteUserAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("Attempt to delete non-existing user with ID: {UserId}", id);
                return false;
            }
            await _userRepository.DeleteUserAsync(id);
            return true;
        }

        // filter users -manager
        public async Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email)
        {
            return await _userRepository.GetFilteredUsersAsync(name, email);
        }


        // Check if user mail exists
        private async Task<bool> UserEmailExistsAsync(string email, int id)
        {
            return await _userRepository.UserEmailExistsAsync(email, id);
        }


        // Authenticate user and generate JWT token
        public async Task<loginResponseDto?> AuthenticateAsync(loginRequestDto loginUser)
        {
            var user = await _userRepository.GetUserByEmail(loginUser.Email);
            {
                if (user == null)
                {
                    _logger.LogWarning("Login attempt failed: User not found for email {Email}", loginUser.Email);
                    return null;
                }
                var hashedPassword = HashPassword(loginUser.Password);

                if (user.Password != hashedPassword)
                {
                    _logger.LogWarning("Login attempt failed: Invalid password for email {Email}", loginUser.Email);
                    return null;
                }

                var token = _tokenService.GenerateToken(user.Id, user.Email, user.First_name, user.Last_name, user.Phone, user.Role);
                var expiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 60);

                _logger.LogInformation("User {UserId} authenticated successfully", user.Id);
                return new loginResponseDto
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiresInMinutes = expiryMinutes * 60,
                    User = _mapper.Map<GetUserDto>(user)
                };

            }
        }
        // Get user by email
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

    }
}
