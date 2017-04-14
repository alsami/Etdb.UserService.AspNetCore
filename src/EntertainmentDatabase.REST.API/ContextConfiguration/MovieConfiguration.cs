using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.ContextConfiguration
{
    public class MovieConfiguration : EntityMappingConfiguration<Movie>
    {
        protected override void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasIndex(movie => movie.Title)
                .IsUnique();

            builder.Property(movie => movie.Title)
                .IsRequired();

            builder.HasMany(movie => movie.MovieFiles)
                .WithOne(movieFIle => movieFIle.Movie)
                .HasForeignKey(movieFile => movieFile.MovieId);
        }
    }
}
