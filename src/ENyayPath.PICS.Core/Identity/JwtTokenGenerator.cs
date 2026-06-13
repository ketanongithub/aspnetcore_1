using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Authorization.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ENyayPath.PICS.Core.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("tenant_id", user.TenantId.ToString())
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 🔹 Add Permission claims from roles
            // If your IdentityManager can fetch them:
            foreach (var permission in PermissionNames.GetAll())
            {
                claims.Add(new Claim("Permission", permission));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Authentication:JwtBearer:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Authentication:JwtBearer:Issuer"],
                audience: _config["Authentication:JwtBearer:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
