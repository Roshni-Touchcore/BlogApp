using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApp.Models.Domain;
using Microsoft.IdentityModel.Tokens;


namespace BlogApp.Repository.Implementation
{
    public static class Authenticator
    {

        public static string CreateToken(User user, DateTime expiresAt, string strSecretKey)
        {
            var userId = user.UserId.ToString();

            var claims = new List<Claim>
            {
                new Claim("UserId",userId),
                new Claim("Username",user.UserName)
            };

            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);



            var jwt = new JwtSecurityToken(
             signingCredentials: new SigningCredentials(
                 new SymmetricSecurityKey(secretKey),
                 SecurityAlgorithms.HmacSha256Signature),
             claims: claims,
             expires: expiresAt,
             notBefore: DateTime.UtcNow
             );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


        public static IEnumerable<Claim>? VerifyToken(string token, string strSecretKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            if (token.StartsWith("Bearer"))
            {
                token = token.Substring(6).Trim();
            }

            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            SecurityToken securityToken;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);

                if (securityToken != null)
                {
                    var tokenObject = tokenHandler.ReadJwtToken(token);
                    return tokenObject.Claims ?? new List<Claim>();
                }
                else
                {
                    return null;
                }
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch
            {
                throw;
            }

        }

    }
}
