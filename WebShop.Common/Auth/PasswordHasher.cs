using System;
using System.Security.Cryptography;

namespace WebShop.Services.Auth
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            hashBytes = Convert.FromBase64String(savedPasswordHash);
            
            salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            hash = pbkdf2.GetBytes(20);
            
            for (int i=0; i < 20; i++)
                if (hashBytes[i+16] != hash[i])
                    throw new UnauthorizedAccessException();
            
            if (savedPasswordHash.Length > 20)
            {
                savedPasswordHash = savedPasswordHash.Substring(0,20);
            }
            
            return savedPasswordHash;
            /*byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (hashed.Length > 20)
            {
                hashed = hashed.Substring(0,20);
            }

            return hashed;*/
        }
    }
}