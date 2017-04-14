using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions
{
    public class MediaMappingConfiguration<T> : EntityMappingConfiguration<T> where T : class, IMediaEntity, new()
    {
        protected override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(mediaEntity => mediaEntity.UniqueName)
                .ForSqlServerHasDefaultValueSql("CONCAT(Id, '-', Name");

            builder.Property(mediaFile => mediaFile.Name)
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
