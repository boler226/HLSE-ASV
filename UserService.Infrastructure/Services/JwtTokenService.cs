using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsersService.Domain.Entities;
using UsersService.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UsersService.Infrastructure.Services {
    public class JwtTokenService(
        IConfiguration config,
        UserManager<User> userManager) : IJwtTokenService {
        private const int tokenLifeTimeInDays = 7;

        public async Task<string> GenerateTokenAsync(User user) {
            var key = Encoding.UTF8.GetBytes(config["Authentication:Jwt:SecretKey"]!);
            var signinKey = new SymmetricSecurityKey(key);
            var signinCredential = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredential,
                expires: DateTime.UtcNow.AddDays(tokenLifeTimeInDays),
                claims: await GetClaimsAsync(user));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<List<Claim>> GetClaimsAsync(User user) {
            var userEmail = user.Email ?? throw new NullReferenceException("User.Email");
            var userRoles = await userManager.GetRolesAsync(user);

            var roleClaims = userRoles
                .Select(role => new Claim(ClaimTypes.Role, role))
                .ToList();

            var claims = new List<Claim>
            {
                new("id", user.Id.ToString()),
                new("email", userEmail),
                new("lastName", user.LastName ?? string.Empty)
            };
            claims.AddRange(roleClaims);

            return claims;
        }
    }
}
