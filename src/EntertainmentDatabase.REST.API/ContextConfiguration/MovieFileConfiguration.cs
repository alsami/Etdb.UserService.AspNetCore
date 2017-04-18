using EntertainmentDatabase.REST.API.ContextConfiguration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.ContextConfiguration
{
    public class MovieFileConfiguration : MediaFileMediaMappingConfiguration<MovieFile>{}
}
