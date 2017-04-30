using EntertainmentDatabase.REST.API.DataAccess.Configuration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    public class MovieFileConfiguration : MediaFileMappingConfiguration<MovieFile>{
        public MovieFileConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}
    }
}
