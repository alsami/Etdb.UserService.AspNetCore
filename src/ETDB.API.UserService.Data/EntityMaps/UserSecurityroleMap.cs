using Etdb.UserService.Domain.Entities;
using ETDB.API.ServiceBase.Repositories.Abstractions.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Etdb.UserService.Data.EntityMaps
{
    internal class UserSecurityroleMap : EntityMapBase<UserSecurityrole>
    {
        public override void Configure(EntityTypeBuilder<UserSecurityrole> builder)
        {
            base.Configure(builder);

            builder.HasOne(userSecurityrole => userSecurityrole.User)
                .WithMany(user => user.UserSecurityroles);

            builder.HasOne(userSecurityrole => userSecurityrole.Securityrole)
                .WithMany(role => role.UserSecurityroles);
        }
    }
}
