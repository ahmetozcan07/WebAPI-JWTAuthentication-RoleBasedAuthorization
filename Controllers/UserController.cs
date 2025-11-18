using JWTandRoleBasedApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTandRoleBasedApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Everyone with a valid token:
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok($"You are: {User.Identity?.Name}");
        }

        // Only Admin:
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_context.Users.ToList());
        }
    }
}
