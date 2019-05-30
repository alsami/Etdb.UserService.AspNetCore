using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class
        ExternalAuthenticationCommandTypeConverter : ITypeConverter<ExternalAuthenticationDto,
            ExternalAuthenticationCommand>
    {
        public ExternalAuthenticationCommand Convert(ExternalAuthenticationDto source,
            ExternalAuthenticationCommand destination, ResolutionContext context) =>
            new ExternalAuthenticationCommand(source.ClientId, source.Token, source.Provider);
    }
}