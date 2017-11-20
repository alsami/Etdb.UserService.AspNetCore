using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Hasher;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Presentation.DataTransferObjects.Resolver
{
    public class RegisterUserPasswordResolver : IValueResolver<RegisterUserDTO, User, string>
    {
        private readonly IHasher hasher;

        public RegisterUserPasswordResolver(IHasher hasher)
        {
            this.hasher = hasher;
        }

        public string Resolve(RegisterUserDTO source, User destination, string destMember, ResolutionContext context)
        {
            var salt = this.hasher.GenerateSalt();

            destination.Salt = salt;

            return this.hasher.CreateSaltedHash(source.Password, salt);
        }
    }
}
