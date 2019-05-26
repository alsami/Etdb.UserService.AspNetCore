using System;
using System.Linq;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserRegisterCommandTypeConverter : ITypeConverter<UserRegisterDto, UserRegisterCommand>
    {
        public UserRegisterCommand Convert(UserRegisterDto source, UserRegisterCommand destination,
            ResolutionContext context)
        {
            var emailsToAdd = source.Emails?.Select(email =>
                    new EmailAddCommand(Guid.NewGuid(), email.Address, email.IsPrimary, false))
                .ToArray();

            var passwordAddCommand = new PasswordAddCommand(source.Password);

            return new UserRegisterCommand(Guid.NewGuid(), source.UserName, source.FirstName, source.Name,
                emailsToAdd, (int) AuthenticationProvider.UsernamePassword, passwordAddCommand);
        }
    }
}