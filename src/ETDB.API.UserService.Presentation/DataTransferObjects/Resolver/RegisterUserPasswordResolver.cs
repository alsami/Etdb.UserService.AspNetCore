using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.ServiceBase.Common.Factory;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ETDB.API.UserService.Presentation.DataTransferObjects.Resolver
{
    public class RegisterUserPasswordResolver : IValueResolver<RegisterUserDTO, User, string>
    {
        public string Resolve(RegisterUserDTO source, User destination, string destMember, ResolutionContext context)
        {
            var hasher = new HasherFactory()
                .CreateHasher(KeyDerivationPrf.HMACSHA1);

            var salt = hasher.GenerateSalt();

            destination.Salt = salt;

            return hasher.CreateSaltedHash(source.Password, salt);
        }
    }
}
