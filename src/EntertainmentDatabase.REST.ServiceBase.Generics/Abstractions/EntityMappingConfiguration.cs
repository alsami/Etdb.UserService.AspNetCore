using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class, IEntity, new()
    {
        protected abstract void Map(EntityTypeBuilder<T> builder);

        public void Map(ModelBuilder builder)
        {
            builder.SetGuidAsPrimaryKey<T>();
            this.Map(builder.Entity<T>());
        }
    }
}
