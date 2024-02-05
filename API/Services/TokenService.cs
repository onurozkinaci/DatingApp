using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        //=>Hem encrypt hem de decrypt islemlerini server yapacagindan SymmetricSecurityKey kullanilabilir,
        //Client'in token key'i decrypt etmesi gerekseydi AsymmetricSecurityKey kullanilirdi(Https ve SSL mantigi gibi).
        //------------------------
        //=>stays in the server, never go to client since client doesnt need to decrypt this token's key;
        private readonly SymmetricSecurityKey _key; 

        public TokenService(IConfiguration config)
        {
            //=>the key which will be used to sign the token is retrieved in the configuration.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            //=>the signature key and the algorithm which signs the token on this key is given;
            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            //=>This part describes the token that we gonna return;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.Now.AddDays(7),
               SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    } 
}