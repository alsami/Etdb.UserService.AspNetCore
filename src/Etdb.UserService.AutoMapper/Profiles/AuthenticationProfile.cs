using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Etdb.UserService.AutoMapper.TypeConverters;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            this.CreateMap<IEnumerable<Claim>, IdentityUserDto>()
                .ConvertUsing<IdentityUserTypeConverter>();
        }
    }
}