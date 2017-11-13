using EntertainmentDatabase.REST.API.WebService.Data.Configuration.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class MovieCoverImageConfiguration : MediaFileMappingConfiguration<MovieFile>{
        public MovieCoverImageConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
    }
}
