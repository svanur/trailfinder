using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TrailFinder.Api;

public class TokenGenerator
{
    public static string GenerateJwtToken()
    {
        var key = Encoding.UTF8.GetBytes("^Nx#F7EuGoU4^QLVEkFsI6FvP1FO7T3@i6zYPKhZ0Y");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "user-id-hér"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // bættu við fleiri claims ef nauðsyn krefur
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            // Set a long expiration time, this effectively makes it "non-expiring"
            Expires = DateTime.Now.AddYears(100),
            Issuer = "http://localhost:4173",
            Audience = "http://localhost:4173",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}