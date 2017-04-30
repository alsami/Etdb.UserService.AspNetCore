using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;

namespace EntertainmentDatabase.REST.API.DataTransferObjects.Mappings
{
    public class MovieFileDTOMapping : Profile
    {
        public MovieFileDTOMapping()
        {
            this.CreateMap<MovieFile, MovieFileDTO>()
                .ForMember(destination => destination.Href, option => option.UseValue("lul"));
            this.CreateMap<MovieFile, MovieFileDTO>()
                .ForMember(destination => destination.File, option => option.MapFrom(source => Convert.ToBase64String(source.File)));
        }
    }
}
