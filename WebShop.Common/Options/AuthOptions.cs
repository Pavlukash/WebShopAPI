using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebShop.Common.Options
{
    public class AuthOptions
    {
        public const string Issuer = "MyAuthServer"; 
        public const string Audience = "MyAuthClient";
        const string Key = "mysupersecret_secretkey!123";
        public const int Lifetime = 15;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}