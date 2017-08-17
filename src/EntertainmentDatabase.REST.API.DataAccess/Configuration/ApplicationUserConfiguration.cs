using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class ApplicationUserConfiguration : EntityMappingConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(entity => entity.UserName)
                .IsUnique();

            builder.Property(entity => entity.UserName)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(entity => entity.Password)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
