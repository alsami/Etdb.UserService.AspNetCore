using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETDB.API.UserService.Data.EntityMaps
{
    internal class SecurityroleMap : EntityMapBase<Securityrole>
    {
        public override void Configure(EntityTypeBuilder<Securityrole> builder)
        {
            base.Configure(builder);

            builder.HasIndex(role => role.Designation)
                .IsUnique();

            builder.Property(role => role.Designation)
                .IsRequired();
        }
    }
}
