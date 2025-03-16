using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Security.Claims;
using System.Text;
using 水水水果API.Models.ConfigurationModel;

namespace 水水水果API.Helper
{
    public class JWTHelper
    {
        private readonly JWTModel _options;

        public JWTHelper(IOptions<JWTModel> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(Member user, int expireMinutes = 10)
        {
            var issuer = _options.JWTIssuer;
            var signKey = _options.JWTSignKey;

            // Configuring "Claims" to your JWT Token
            if( user.LoginRole == null)
            user.LoginRole = "User";
            var claims = new List<Claim>
            {
                //claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName + user.LastName), // User.Identity.Name
                //claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "The Audience"));
                //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
                //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
                //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID

                // The "NameId" claim is usually unnecessary.
                //claims.Add(new Claim(JwtRegisteredClaimNames.NameId, userName));

                // This Claim can be replaced by JwtRegisteredClaimNames.Sub, so it's redundant.
                //claims.Add(new Claim(ClaimTypes.Name, userName));

                // TODO: You can define your "roles" to your Claims.
                
                new Claim(ClaimTypes.Role, user.LoginRole),
                new Claim("MemberId",user.Id.ToString())
            };

            var userClaimsIdentity = new ClaimsIdentity(claims);

            // Create a SymmetricSecurityKey for JWT Token signatures
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

            // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
            // https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                //Audience = issuer, // Sometimes you don't have to define Audience.
                //NotBefore = DateTime.Now, // Default is DateTime.Now
                //IssuedAt = DateTime.Now, // Default is DateTime.Now
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(expireMinutes),
                SigningCredentials = signingCredentials
            };

            // Generate a JWT, than get the serialized Token result (string)
            var tokenHandler = new JsonWebTokenHandler();
            var serializeToken = tokenHandler.CreateToken(tokenDescriptor);

            return serializeToken;
        }
    }
}
