using System.Linq;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserDtoTypeConverter : ITypeConverter<User, UserDto>
    {
        private readonly IProfileImageUrlFactory profileImageUrlFactory;

        public UserDtoTypeConverter(IProfileImageUrlFactory profileImageUrlFactory)
        {
            this.profileImageUrlFactory = profileImageUrlFactory;
        }

        public UserDto Convert(User source, UserDto destination, ResolutionContext context)
            => new UserDto(source.Id, source.UserName,
                source.FirstName,
                source.Name,
                source.Biography,
                source.RegisteredSince,
                source.ProfileImages.Any()
                    ? this.profileImageUrlFactory.GenerateUrl(source)
                    : null,
                source.AuthenticationProvider.ToString(),
                source.AuthenticationProvider != AuthenticationProvider.UsernamePassword,
                source.Emails.Select(email => new EmailDto(email.Id, email.Address, email.IsPrimary)).ToArray());
    }
}