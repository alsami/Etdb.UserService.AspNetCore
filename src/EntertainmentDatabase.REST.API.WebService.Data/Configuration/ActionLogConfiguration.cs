using EntertainmentDatabase.REST.API.WebService.Data.Configuration.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.WebService.Data.Configuration
{
    internal class ActionLogConfiguration : LogInfoMappingConfiguration<ActionLog>
    {
        public ActionLogConfiguration(ModelBuilder modelBuilder) : base(modelBuilder){}

        protected override void Configure(EntityTypeBuilder<ActionLog> builder)
        {
            base.Configure(builder);

            //builder.ToTable($"{nameof(ActionLog)}s");

            builder.Property(actionLog => actionLog.TraceStart)
                .IsRequired();

            builder.Property(actionLog => actionLog.TraceEnd)
                .IsRequired();
        }
    }
}
