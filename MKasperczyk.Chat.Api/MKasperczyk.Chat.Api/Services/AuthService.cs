using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MKasperczyk.Chat.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MKasperczyk.Chat.Api.Services
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool VerifyPassword(TokenRequest tokenRequest, string passwordFromDb)
        {
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(tokenRequest.UserName, passwordFromDb, tokenRequest.Password);
            return passwordVerification != PasswordVerificationResult.Success;
        }

        public string GetToken(int iduser, string userName)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", iduser.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Name, userName),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
