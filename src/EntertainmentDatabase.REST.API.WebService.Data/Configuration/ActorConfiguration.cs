using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class ActorConfiguration : EntityMappingConfiguration<Actor>
    {
        protected override void Configure(EntityTypeBuilder<Actor> builder)
        {
            base.Configure(builder);

            //builder.ToTable($"{nameof(Actor)}s");

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
