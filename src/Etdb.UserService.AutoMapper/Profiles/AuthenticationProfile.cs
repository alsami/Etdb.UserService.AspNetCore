using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Etdb.UserService.AutoMapper.Converters;
using Etdb.UserService.Cqrs.Abstractions.Commands.Authentication;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            this.CreateMap<IEnumerable<Claim>, IdentityUserDto>()
                .ConvertUsing<IdentityUserTypeConverter>();

            this.CreateMap<InternalAuthenticationDto, InternalAuthenticationCommand>()
                .ConvertUsing<InternalAuthenticationCommandTypeConverter>();

            this.CreateMap<ExternalAuthenticationDto, ExternalAuthenticationCommand>()
                .ConvertUsing<ExternalAuthenticationCommandTypeConverter>();
        }
    }
}