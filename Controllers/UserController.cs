using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordHashingDemo.Models;

namespace PasswordHashingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDBContext _context;
        public UserController(UserDBContext context)
        {
            _context = context;    
        }
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDTO model)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash(model.Password,out passwordHash, out passwordSalt);
            User user = new User
            {
                Username = model.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == model.UserName);
            if (user == null || !PasswordHasher.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Invalid Username and Password");
            }
            return Ok("User logged in successfully");
        }
    }
    
    
}
