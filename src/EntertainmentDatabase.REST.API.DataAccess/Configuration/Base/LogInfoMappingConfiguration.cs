using System;
using System.Collections.Generic;
using System.Text;
using EntertainmentDatabase.REST.API.Domain.Base;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.DataAccess.Configuration.Base
{
    internal abstract class LogInfoMappingConfiguration<T> : EntityMappingConfiguration<T> where T : class, ILogInfo, new()
    {
        protected LogInfoMappingConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(errorLog => errorLog.Path)
                .IsRequired();
            builder.Property(errorLog => errorLog.HttpMethod)
                .IsRequired();
            builder.Property(errorLog => errorLog.TraceId)
                .IsRequired();
        }
    }
}
