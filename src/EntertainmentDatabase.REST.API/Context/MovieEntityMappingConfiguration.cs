using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.Context.Mappings
{
    public class MovieEntityMappingConfiguration : EntityMappingConfiguration<Movie>
    {
        protected override void Map(EntityTypeBuilder<Movie> builder)
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
