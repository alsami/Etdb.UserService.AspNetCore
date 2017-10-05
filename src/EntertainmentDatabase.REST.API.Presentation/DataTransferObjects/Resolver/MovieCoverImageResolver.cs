using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Resolver
{
    public class MovieCoverImageResolver : IValueResolver<Movie, MovieDTO, string>
    {
        public string Resolve(Movie source, MovieDTO destination, string destMember, ResolutionContext context)
        {
            return string.Empty;
        }
    }
}
