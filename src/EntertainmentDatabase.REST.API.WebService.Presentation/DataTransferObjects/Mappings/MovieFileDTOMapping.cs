using System;
using AutoMapper;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;

namespace EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects.Mappings
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
