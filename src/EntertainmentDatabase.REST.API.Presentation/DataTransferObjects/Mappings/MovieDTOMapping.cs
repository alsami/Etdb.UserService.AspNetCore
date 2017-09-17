using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Mappings
{
    public class MovieDTOMapping : Profile
    {
        private readonly IHttpContextAccessor httpContextAccesor;

        public MovieDTOMapping(IHttpContextAccessor httpContextAccesor)
        {
            this.CreateMap<Movie, MovieDTO>()
                .ForMember(destination => destination.ConcurrencyToken,
                    option => option.MapFrom(source => source.RowVersion))
                .ForMember(desitination => desitination.MovieCoverImageUrl,
                    options => options.ResolveUsing((src, dest, destValue, ctx) =>
                    {
                        return src.MovieCoverImage != null ? 
                            $"http://{this.httpContextAccesor.HttpContext.Request.Host}/api/main/v1/movies/{src.Id}/moviecoverimages/download/{src.MovieCoverImage.Id}" 
                            : null;
                    }))
                .ReverseMap()
                .ForMember(source => source.RowVersion,
                    option => option.MapFrom(destination => destination.ConcurrencyToken));
            this.httpContextAccesor = httpContextAccesor;
        }
    }
}
