using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class
        UserExternalAuthenticationCommandTypeConverter : ITypeConverter<UserExternalAuthenticationDto,
            ExternalAuthenticationCommand>
    {
        public ExternalAuthenticationCommand Convert(UserExternalAuthenticationDto source,
            ExternalAuthenticationCommand destination, ResolutionContext context) =>
            new ExternalAuthenticationCommand(source.ClientId, source.Token, source.Provider);
    }
}