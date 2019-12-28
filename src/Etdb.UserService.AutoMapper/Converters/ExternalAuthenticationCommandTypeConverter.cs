using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Presentation.Authentication;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class
        ExternalAuthenticationCommandTypeConverter : ITypeConverter<ExternalAuthenticationDto,
            ExternalAuthenticationCommand>
    {
        private readonly IHttpContextAccessor? httpContextAccessor;

        public ExternalAuthenticationCommandTypeConverter(IHttpContextAccessor? httpContextAccessor = null)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ExternalAuthenticationCommand Convert(ExternalAuthenticationDto source,
            ExternalAuthenticationCommand destination, ResolutionContext context) =>
            new ExternalAuthenticationCommand(source.ClientId, source.Token, source.Provider, this.httpContextAccessor?.HttpContext.Connection.RemoteIpAddress);
    }
}