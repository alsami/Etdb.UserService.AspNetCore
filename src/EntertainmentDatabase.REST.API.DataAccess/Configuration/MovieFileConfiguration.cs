using EntertainmentDatabase.REST.API.DataAccess.Configuration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class MovieFileConfiguration : MediaFileMappingConfiguration<MovieFile>{
        public MovieFileConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<MovieFile> builder)
        {
            base.Configure(builder);
        }
    }
}
