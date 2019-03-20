using System.Linq;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.AutoMapper.TypeConverters
{
    public class UserRegisterCommandTypeConverter : ITypeConverter<UserRegisterDto, UserRegisterCommand>
    {
        public UserRegisterCommand Convert(UserRegisterDto source, UserRegisterCommand destination,
            ResolutionContext context)
        {
            var emailsToAdd = source.Emails?.Select(email => new EmailAddCommand(email.Address, email.IsPrimary, false))
                .ToArray();

            var passwordAddCommand = new PasswordAddCommand(source.Password);

            return new UserRegisterCommand(source.UserName, source.FirstName, source.Name,
                emailsToAdd, (int) AuthenticationProvider.UsernamePassword, passwordAddCommand);
        }
    }
}