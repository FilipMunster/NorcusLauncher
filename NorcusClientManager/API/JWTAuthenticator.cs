using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager.API
{
    internal class JWTAuthenticator : ITokenAuthenticator
    {
        private string _key;
        public JWTAuthenticator(string secureKey)
        {
            _key = secureKey;
        }
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;


            TokenValidationParameters tokenValidationParameters = _GetTokenValidationParameters();
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private SecurityKey _GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Encoding.ASCII.GetBytes(_key);
            return new SymmetricSecurityKey(symmetricKey);
        }
        private TokenValidationParameters _GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = _GetSymmetricSecurityKey()
            };
        }
    }
}
