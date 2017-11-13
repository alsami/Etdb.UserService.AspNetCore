using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class MovieConfiguration : EntityMappingConfiguration<Movie>
    {
        public MovieConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<Movie> builder)
        {
            base.Configure(builder);

            //builder.ToTable($"{nameof(Movie)}s");

            builder.HasIndex(movie => movie.Title)
                .IsUnique();

            builder.Property(movie => movie.Title)
                .IsRequired();

            builder.HasOne(movie => movie.MovieCoverImage)
                .WithOne(movieCoverImage => movieCoverImage.Movie)
                .HasForeignKey<MovieCoverImage>(movieCoverImage => movieCoverImage.MovieId);

            builder.HasMany(movie => movie.MovieFiles)
                .WithOne(movieFIle => movieFIle.Movie)
                .HasForeignKey(movieFile => movieFile.MovieId);
        }
    }
}
