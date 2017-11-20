using ETDB.API.ServiceBase.Entities;
using ETDB.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETDB.API.UserService.Data.Configuration
{
    internal class UserMappingConfiguration : EntityMappingConfiguration<User>
    {
        public UserMappingConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasIndex(user => user.UserName)
                .IsUnique();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.UserName)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(user => user.Email)
                .IsRequired();

            builder.Property(user => user.Name)
                .IsRequired();

            builder.Property(user => user.LastName)
                .IsRequired();

            builder.Property(user => user.Password)
                .IsRequired();

            builder.Property(user => user.Salt)
                .IsRequired();

            builder.Property(user => user.IsActive)
                .HasDefaultValue(true);
        }
    }
}
