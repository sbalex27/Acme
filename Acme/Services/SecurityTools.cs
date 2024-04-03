using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Acme.Services
{
    public class SecurityTools
    {
        private const string SecurityPepper = "Security:Pepper";
        private readonly IConfiguration _configuration;

        public SecurityTools(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CompareHash(string password, string salt, string hash)
        {
            var computedHash = ComputeHash(password, salt);
            return computedHash == hash;
        }

        public string ComputeHash(string password, string salt, int iteration = 3)
        {
            if (iteration <= 0) return password;

            var pepper = GetPepper();
            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = SHA256.HashData(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, iteration - 1);
        }

        public string? GetPepper()
        {
            return _configuration.GetSection(SecurityPepper).Get<string>();
        }

        public string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }

        public string GenerateJwt(int minutes = 120)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(minutes),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}
