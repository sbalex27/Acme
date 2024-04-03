using Acme.Data;
using Acme.Models;
using Acme.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Acme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AcmeContext _context;
        private readonly SecurityTools _securityTools;

        public AuthController(AcmeContext context, SecurityTools securePasswordHash)
        {
            _context = context;
            _securityTools = securePasswordHash;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest user)
        {
            if (await _context.User.AnyAsync(u => u.UserName == user.UserName))
            {
                return BadRequest("Ya existe este nombre de usuario");
            }

            var salt = _securityTools.GenerateSalt();
            var password = _securityTools.ComputeHash(user.Password, salt);

            _context.User.Add(new User
            {
                UserName = user.UserName,
                Password = password,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();

            var token = _securityTools.GenerateJwt();

            return Ok(new AuthResponse(token));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest user)
        {
            var dbUser = await _context.User.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (dbUser == null)
            {
                return BadRequest("Usuario o contraseña incorrectos");
            }

            if (!_securityTools.CompareHash(user.Password, dbUser.Salt, dbUser.Password))
            {
                return BadRequest("Usuario o contraseña incorrectos");
            }

            var token = _securityTools.GenerateJwt();

            return Ok(new AuthResponse(token));
        }
    }

    public record RegisterRequest([field: Required] string UserName, [field: Required] string Password);
    public record LoginRequest([field: Required] string UserName, [field: Required] string Password);
    public record AuthResponse(string Token);
}

