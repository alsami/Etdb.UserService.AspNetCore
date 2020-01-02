using AutoMapper;
using Etdb.UserService.AutoMapper.Converters;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class AuthenticationLogProfie : Profile
    {
        public AuthenticationLogProfie()
        {
            this.CreateMap<UserAuthenticatedEvent, AuthenticationLog>()
                .ConvertUsing<AuthenticationLogConverter>();

            this.CreateMap<AuthenticationLog, AuthenticationLogDto>()
                .ConstructUsing(authenticationLog => new AuthenticationLogDto(authenticationLog.LoggedAt,
                    authenticationLog.AuthenticationLogType.ToString(), authenticationLog.IpAddress,
                    authenticationLog.AdditionalInformation));
        }
    }
}