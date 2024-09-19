using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Authentication.Authentication;
using MovieBookingSystem.Authentication.DTO;
using MovieBookingSystem.Authentication.IRepository;
using MovieBookingSystem.Authentication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieBookingSystem.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserContext _dbContext;
        private readonly IUserRepository _userRepository;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public AuthController(IUserRepository userRepository, UserContext dbContext, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDetails user)
        {
            bool IsEmailExist = await _userRepository.ValidateUserEmail(user);

            if (IsEmailExist)
            {
                return BadRequest("User already exists");
            }
            var addUser = await _userRepository.AddUserDetails(user);
            if(addUser.Equals("Details added successfully"))
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDetails user)
        {
            bool IsEmailExist = await _userRepository.ValidateUserEmail(user);
            if (!IsEmailExist)
            {
                return NotFound();
            }

            var hashedPassword = await _userRepository.GetHashedPassword(user.Email);
            if (!SecurePasswordHasherHelper.Verify(user.Password, hashedPassword))
            {
                return Unauthorized();
            }

            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, user.Role)
                 };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_role = user.Role,
            });

        }
    }
}
