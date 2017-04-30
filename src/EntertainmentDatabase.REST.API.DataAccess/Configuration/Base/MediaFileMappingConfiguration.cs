using EntertainmentDatabase.REST.API.Domain.Base;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration.Base
{
    public abstract class MediaFileMappingConfiguration<T> : EntityMappingConfiguration<T> where T : class, IMediaFile, new()
    {
        protected override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(mediaFile => mediaFile.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(mediaFile => mediaFile.Extension)
                .IsRequired()
                .HasMaxLength(16);
        }
    }
}
