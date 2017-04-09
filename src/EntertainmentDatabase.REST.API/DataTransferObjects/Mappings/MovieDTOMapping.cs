using AutoMapper;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;

namespace EntertainmentDatabase.REST.API.DataTransferObjects.Mappings
{
    public class MovieDTOMapping : Profile
    {
        public MovieDTOMapping()
        {
            this.CreateMap<Movie, MovieDTO>()
                .ForMember(destination => destination.ConcurrencyToken, option => option.MapFrom(source => source.RowVersion))
                .ReverseMap();
        }
    }
}
