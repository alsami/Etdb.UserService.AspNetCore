using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Entities;

namespace EntertainmentDatabase.REST.API.DataTransferObjects.Mappings
{
    public class ActorDTOMapping : Profile
    {
        public ActorDTOMapping()
        {
            this.CreateMap<Actor, ActorDTO>()
                .ForMember(destination => destination.ConcurrencyToken,
                    option => option.MapFrom(source => source.RowVersion))
                .ForMember(destination => destination.FullName,
                    option => option.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ReverseMap();
        }
    }
}
