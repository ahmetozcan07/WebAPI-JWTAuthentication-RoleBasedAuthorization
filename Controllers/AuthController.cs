using JWTandRoleBasedApp.Data;
using JWTandRoleBasedApp.Helpers;
using JWTandRoleBasedApp.Models;
using JWTandRoleBasedApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTandRoleBasedApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest request)
        {
            if (request.Username == null || request.Password == null)
                return BadRequest("Username and password cannot be empty.");

            var exists = await _context.Users.AnyAsync(x => x.Username == request.Username);
            if (exists)
                return BadRequest("User already exists");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request.Username == null || request.Password == null)
                return BadRequest("Username and password cannot be empty.");

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == request.Username);
            if (user == null)
                return Unauthorized("User not found");

            if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid password or username");

            var token = _jwtService.GenerateToken(user);
            return Ok(token);
        }
    }
}
