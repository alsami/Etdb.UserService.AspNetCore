using EntertainmentDatabase.REST.API.WebService.Data.Configuration.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class MovieCoverImageConfiguration : MediaFileMappingConfiguration<MovieCoverImage>
    {

        public MovieCoverImageConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<MovieCoverImage> builder)
        {
            base.Configure(builder);

            //builder.ToTable($"{nameof(MovieCoverImage)}s");
        }
    }
}
