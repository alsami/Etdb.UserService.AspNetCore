using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.DataTransferObjects;
using EntertainmentDatabase.REST.Domain.Entities;

namespace EntertainmentDatabase.REST.API.Mappings
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            this.CreateMap<Movie, MovieDTO>()
                .ReverseMap();
        }
    }
}
