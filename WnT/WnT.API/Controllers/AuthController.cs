using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WnT.API.Models.DTO.auth;
using WnT.API.Models.DTO.region;
using WnT.API.Repo.region;
using WnT.API.Repo.token;

namespace WnT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepo tokenRepo;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepo tokenRepo)
        {
            this.userManager = userManager;
            this.tokenRepo = tokenRepo;

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {

            var identityUser = new IdentityUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
            };

            var createResult = await userManager.CreateAsync(identityUser, registerDTO.Password);


            if (createResult.Succeeded)
            {
                if (registerDTO.roles != null && registerDTO.roles.Any())
                {
                    var addRolesResult = await userManager.AddToRolesAsync(identityUser, registerDTO.roles);
                    if (addRolesResult.Succeeded)
                    {
                        return Ok("Registration Succeeded");
                    }
                    else
                    {
                        // Return the errors if adding roles fails
                        return BadRequest(string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));
                    }
                }
                return Ok("Registration succeeded without roles.");
            }

            return BadRequest(string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // Check if the user exists
            var user = await userManager.FindByEmailAsync(loginDTO.UserName);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Verify the password
            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            // Retrieve user roles
            var roles = await userManager.GetRolesAsync(user);

            // Generate token (consider handling any potential failure in token generation)
            var token = tokenRepo.GenerateJWTToken(user, roles.ToList());
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to generate token." });
            }

            var response = new LoginResponseDTO
            { 
                JwtToken = token
            };

            // Successful response with the generated token
            return Ok(response);
        }

    }
}
