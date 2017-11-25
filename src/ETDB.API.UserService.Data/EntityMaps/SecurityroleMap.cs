using ETDB.API.ServiceBase.Entities;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETDB.API.UserService.Data.EntityMaps
{
    internal class SecurityroleMap : EntityMapBase<Securityrole>
    {
        public override void Configure(EntityTypeBuilder<Securityrole> builder)
        {
            base.Configure(builder);

            builder.Property(role => role.Designation)
                .IsRequired();
        }
    }
}
