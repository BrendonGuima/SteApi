using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SteWebApi.Model;

namespace SteWebApi.Services;

public class JwtTokenGeneration
{
    public string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("eyH31298ehR2wef329Rh923bF9923ED1");
        var claims = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}