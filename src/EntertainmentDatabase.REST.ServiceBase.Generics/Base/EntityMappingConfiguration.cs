using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Base
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class, IEntity, new()
    {
        protected readonly ModelBuilder ModelBuilder;

        protected EntityMappingConfiguration(ModelBuilder modelBuilder)
        {
            this.ModelBuilder = modelBuilder;
        }

        protected abstract void Configure(EntityTypeBuilder<T> builder);

        public virtual void ConfigureEntity()
        {
            this.AutoAddGuidPrimaryKey();
            this.EnableConccurentTracking();
            this.Configure(this.ModelBuilder.Entity<T>());
        }

        protected virtual void AutoAddGuidPrimaryKey()
        {
            this.ModelBuilder.Entity<T>(builder =>
            {
                builder.HasKey(entity => entity.Id);

                builder.Property(entity => entity.Id)
                    .ForSqlServerHasDefaultValueSql("newid()");
            });
        }

        protected virtual void EnableConccurentTracking()
        {
            this.ModelBuilder.Entity<T>(builder =>
            {
                builder.Property(entity => entity.RowVersion)
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();
            });
        }
    }
}
