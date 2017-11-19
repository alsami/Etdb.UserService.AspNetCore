using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DataTransferObjects.Resolver;

namespace ETDB.API.UserService.Presentation.DataTransferObjects.Mappings
{
    public class RegisterUserDTOMapping : Profile
    {
        public RegisterUserDTOMapping()
        {
            this.CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.Salt, option => option.Ignore())
                .ForMember(dest => dest.Password, option => option.ResolveUsing<RegisterUserPasswordResolver>());
        }
    }
}
