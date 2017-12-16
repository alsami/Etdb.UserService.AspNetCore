using Etdb.ServiceBase.Repositories.Abstractions.Base;
using Etdb.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Etdb.UserService.Data.EntityMaps
{
    internal class UserMap : EntityMapBase<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasIndex(user => user.UserName)
                .IsUnique();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.Salt)
                .IsRequired();
        }
    }
}
