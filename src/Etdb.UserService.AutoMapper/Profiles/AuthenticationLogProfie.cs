using AutoMapper;
using Etdb.UserService.AutoMapper.Converters;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class AuthenticationLogProfie : Profile
    {
        public AuthenticationLogProfie()
        {
            this.CreateMap<UserAuthenticatedEvent, AuthenticationLog>()
                .ConvertUsing<AuthenticationLogConverter>();
        }
    }
}