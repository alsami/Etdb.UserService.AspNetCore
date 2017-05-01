using System;
using System.Collections.Generic;
using System.Text;
using EntertainmentDatabase.REST.API.DataAccess.Configuration.Base;
using EntertainmentDatabase.REST.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration
{
    internal class ActionLogConfiguration : LogInfoMappingConfiguration<ActionLog>
    {
        public ActionLogConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<ActionLog> builder)
        {
            base.Configure(builder);

            builder.Property(actionLog => actionLog.TraceStart)
                .IsRequired();

            builder.Property(actionLog => actionLog.TraceEnd)
                .IsRequired();
        }
    }
}
