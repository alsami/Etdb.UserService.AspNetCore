using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class, IEntity, new()
    {
        protected abstract void Configure(EntityTypeBuilder<T> builder);

        public virtual void Configure(ModelBuilder builder)
        {
            builder.UseGuidPrimaryKey<T>();
            builder.UseConccurencyToken<T>();
            this.Configure(builder.Entity<T>());
        }
    }
}
