using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.UserService.Data.Configuration
{
    internal class SecurityroleMappingConfiguration : EntityMappingConfiguration<Securityrole>
    {
        public SecurityroleMappingConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<Securityrole> builder)
        {
            base.Configure(builder);

            builder.Property(role => role.Designation)
                .IsRequired();

            builder.HasIndex(role => role.Designation)
                .IsUnique();
        }
    }
}
