using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookManagementSystem.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        public TokenService(UserManager<User> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<string> GenerateJwtToken(User appUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id)
            }
            .Union(roleClaims)
            .Union(userClaims);

            var symmertricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
            var signingCredentials = new SigningCredentials(symmertricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public string? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
    }
}
