using System;
using System.Collections.Generic;
using System.Text;
using EntertainmentDatabase.REST.API.ServiceBase.Common.Base;
using EntertainmentDatabase.REST.API.ServiceBase.Common.Hasher;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace EntertainmentDatabase.REST.API.ServiceBase.Common.Factory
{
    public class HasherFactory
    {
        public IHashingStrategy CreateHasher(KeyDerivationPrf keyDerivation)
        {
            switch (keyDerivation)
            {
                case KeyDerivationPrf.HMACSHA1:
                    return new Hmacsha1HashingStrategy();
                case KeyDerivationPrf.HMACSHA256:
                    throw new ArgumentOutOfRangeException(nameof(keyDerivation), keyDerivation, null);
                case KeyDerivationPrf.HMACSHA512:
                    throw new ArgumentOutOfRangeException(nameof(keyDerivation), keyDerivation, null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyDerivation), keyDerivation, null);
            }
        }
    }
}
