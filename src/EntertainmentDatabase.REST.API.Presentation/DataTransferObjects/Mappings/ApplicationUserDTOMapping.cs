using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects.Mappings
{
    public class ApplicationUserDTOMapping : Profile
    {
        public ApplicationUserDTOMapping()
        {
            this.CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(destination => destination.ConcurrencyToken,
                    option => option.MapFrom(source => source.RowVersion))
                .ReverseMap();
        }
    }
}
