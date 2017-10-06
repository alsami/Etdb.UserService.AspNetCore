using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Resolver
{
    public class MovieCoverImageResolver : IValueResolver<Movie, MovieDTO, string>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MovieCoverImageResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Movie source, MovieDTO destination, string destMember, ResolutionContext context)
        {
            return "FUCK YOU";
        }
    }
}
