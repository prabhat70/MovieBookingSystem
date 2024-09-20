using AuthenticationPlugin;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.DAL;
using UserService.DTO;
using UserService.IRepository;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext _dbContext;
        private readonly IUserRepository _userRepository;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UserController(IUserRepository userRepository, UserContext dbContext, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration user)
        {
            bool IsEmailExist = await _userRepository.ValidateUserEmail(user.Email);

            if (IsEmailExist)
            {
                return BadRequest("User already exists");
            }
            var addUser = await _userRepository.AddUserDetails(user);
            if(addUser.Equals("Details added successfully"))
            {
                return Ok("Account created successfully.");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserSignIn user)
        {
            bool IsEmailExist = await _userRepository.ValidateUserEmail(user.Email);
            if (!IsEmailExist)
            {
                return NotFound();
            }

            var userDetails = await _userRepository.GetUserDetails(user.Email);
            if (!SecurePasswordHasherHelper.Verify(user.Password, userDetails.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, userDetails.Email),
                   new Claim(ClaimTypes.Email, userDetails.Email),
                   new Claim(ClaimTypes.Role, userDetails.Role)
                 };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_role = userDetails.Role
            });

        }

        [HttpPost("deactivate-account")]
        public async Task<IActionResult> DeactivateAccount([FromBody] UserSignIn user)
        {
            bool IsEmailExist = await _userRepository.ValidateUserEmail(user.Email);
            if (!IsEmailExist)
            {
                return NotFound();
            }

            var userDetails = await _userRepository.GetUserDetails(user.Email);
            if (!SecurePasswordHasherHelper.Verify(user.Password, userDetails.Password))
            {
                return Unauthorized();
            }

            var res = await _userRepository.DeactivateAccount(user.Email);
            if (res)
            {
                return Ok("Account deactivated successfully.");
            }
            return BadRequest("Something went wrong");
        }
    }
}
