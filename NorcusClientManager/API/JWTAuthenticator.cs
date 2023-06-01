using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool Authenticate(string token)
        {
            return true;
        }
    }
}
