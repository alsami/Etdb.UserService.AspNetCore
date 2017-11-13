using System;
using System.Security.Cryptography;
using EntertainmentDatabase.REST.API.ServiceBase.Common.Base;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace EntertainmentDatabase.REST.API.ServiceBase.Common.Hasher
{
    public class Hmacsha1HashingStrategy : IHashingStrategy
    {
        public string CreateSaltedHash(string unhashed, byte[] salt)
        {
            if (unhashed == null) throw new ArgumentNullException(nameof(unhashed));

            if (salt == null) throw new ArgumentNullException(nameof(salt));

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(unhashed, 
                salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));
        }

        public byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(salt);
            }

            return salt;
        }
    }
}
