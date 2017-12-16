using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Etdb.UserService.Data.EntityMaps
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
