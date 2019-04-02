using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class
        UserInternalAuthenticationCommandTypeConverter : ITypeConverter<UserInternalAuthenticationDto,
            InternalAuthenticationCommand>
    {
        public InternalAuthenticationCommand Convert(UserInternalAuthenticationDto source,
            InternalAuthenticationCommand destination, ResolutionContext context) =>
            new InternalAuthenticationCommand(source.Username, source.Password, source.ClientId);
    }
}