using ETDB.API.ServiceBase.Entities;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETDB.API.UserService.Data.Configuration
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
