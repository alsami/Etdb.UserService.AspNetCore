using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Mappings
{
    public class ApplicationUserDTOMapping : Profile
    {
        public ApplicationUserDTOMapping()
        {
            this.CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ReverseMap();
        }
    }
}
