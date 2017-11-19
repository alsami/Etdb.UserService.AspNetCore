using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Presentation.DataTransferObjects.Mappings
{
    public class UserDTOMapping : Profile
    {
        public UserDTOMapping()
        {
            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.ConccurencyToken, option => option.MapFrom(source => source.RowVersion))
                .ReverseMap()
                .ForMember(dest => dest.RowVersion, option => option.MapFrom(source => source.ConccurencyToken));
        }
    }
}
