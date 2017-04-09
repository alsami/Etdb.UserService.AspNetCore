using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;
using EntertainmentDatabase.REST.ServiceBase.DataAccess.Facades;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Scaffolding.Configuration.Internal;

namespace EntertainmentDatabase.REST.API.Context
{
    public class MovieEntityMappingConfiguration : EntityMappingConfiguration<Movie>
    {
        public override void Map(EntityTypeBuilder<Movie> builder)
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
