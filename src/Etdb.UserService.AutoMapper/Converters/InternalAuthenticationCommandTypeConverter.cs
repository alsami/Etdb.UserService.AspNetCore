using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class
        InternalAuthenticationCommandTypeConverter : ITypeConverter<InternalAuthenticationDto,
            InternalAuthenticationCommand>
    {
        private readonly IHttpContextAccessor? httpContextAccessor;

        public InternalAuthenticationCommandTypeConverter(IHttpContextAccessor? httpContextAccessor = null)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public InternalAuthenticationCommand Convert(InternalAuthenticationDto source,
            InternalAuthenticationCommand destination, ResolutionContext context) =>
            new InternalAuthenticationCommand(source.Username, source.Password, source.ClientId,
                AuthenticationProvider.UsernamePassword.ToString(),
                this.httpContextAccessor?.HttpContext.Connection.RemoteIpAddress);
    }
}