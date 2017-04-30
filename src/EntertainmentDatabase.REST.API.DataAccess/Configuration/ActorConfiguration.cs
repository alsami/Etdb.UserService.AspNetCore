using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class ActorConfiguration : EntityMappingConfiguration<Actor>
    {
        protected override void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(actor => actor.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(actor => actor.LastName)
                .HasMaxLength(128)
                .IsRequired();
        }

        public ActorConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}
    }
}
