using AutoMapper;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects.Resolver;

namespace EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects.Mappings
{
    public class MovieDTOMapping : Profile
    {

        public MovieDTOMapping()
        {
            this.CreateMap<Movie, MovieDTO>()
                .ForMember(destination => destination.ConcurrencyToken,
                    option => option.MapFrom(source => source.RowVersion))
                .ForMember(destination => destination.MovieCoverImageUrl,
                    option => option.ResolveUsing<MovieCoverImageResolver>())
                //.ForMember(desitination => desitination.MovieCoverImageUrl,
                //    options => options.ResolveUsing((src, dest, destValue, ctx) =>
                //    {
                //        return src.MovieCoverImage != null ? 
                //            $"http://{this.httpContextAccesor.HttpContext.Request.Host}/api/main/v1/movies/{src.Id}/moviecoverimages/download/{src.MovieCoverImage.Id}" 
                //            : null;
                //    }))
                .ReverseMap()
                .ForMember(source => source.RowVersion,
                    option => option.MapFrom(destination => destination.ConcurrencyToken));
        }
    }
}
