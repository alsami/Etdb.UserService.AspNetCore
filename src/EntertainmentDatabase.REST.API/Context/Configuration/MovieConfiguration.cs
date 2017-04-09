using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.Context.Configuration
{
    public class MovieConfiguration : EntityMappingConfiguration<Movie>
    {
        protected override void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(movie => movie.RowVersion)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            builder.HasIndex(movie => movie.Title)
                .IsUnique();

            builder.Property(movie => movie.Title)
                .IsRequired();
        }
    }
}
