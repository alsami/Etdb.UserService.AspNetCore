using System;
using System.Collections.Generic;
using System.Text;
using EntertainmentDatabase.REST.API.DataAccess.Configuration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class ErrorLogConfiguration : LogInfoMappingConfiguration<ErrorLog>
    {
        public ErrorLogConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            base.Configure(builder);
            builder.Property(errorLog => errorLog.Message)
                .IsRequired()
                .HasMaxLength(4096);
        }
    }
}
