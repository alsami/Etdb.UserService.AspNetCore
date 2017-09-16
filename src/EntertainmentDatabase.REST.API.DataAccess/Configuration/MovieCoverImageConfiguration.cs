using EntertainmentDatabase.REST.API.DataAccess.Configuration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class MovieCoverImageConfiguration : MediaFileMappingConfiguration<MovieFile>{
        public MovieCoverImageConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
    }
}
