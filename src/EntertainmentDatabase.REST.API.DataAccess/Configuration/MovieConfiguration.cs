using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    public class MovieConfiguration : EntityMappingConfiguration<Movie>
    {
        public MovieConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

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
