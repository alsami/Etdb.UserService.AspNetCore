using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.DataTransferObjects;
using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;

namespace EntertainmentDatabase.REST.API.Mappings
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            this.CreateMap<Movie, MovieDTO>()
                .ForMember(destination => destination.ConcurrencyToken, option => option.MapFrom(source => source.RowVersion))
                .ReverseMap();
        }
    }
}
