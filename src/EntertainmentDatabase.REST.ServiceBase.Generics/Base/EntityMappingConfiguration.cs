using System.Linq;
using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Base
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class, IEntity, new()
    {
        private readonly ModelBuilder modelBuilder;

        protected abstract void Configure(EntityTypeBuilder<T> builder);

        public virtual void Configure(ModelBuilder builder)
        {
            builder.UseGuidPrimaryKey<T>();
            builder.UseConccurencyToken<T>();
            this.Configure(builder.Entity<T>());
        }

        //public void UseGuidPrimaryKey<T>(this ModelBuilder modelBuilder) where T : class, IEntity, new()
        //{
        //    modelBuilder.Entity<T>(builder =>
        //    {
        //        builder.HasKey(entity => entity.Id);

        //        builder.Property(entity => entity.Id)
        //            .ForSqlServerHasDefaultValueSql("newid()");
        //    });
        //}

        //public void UseConccurencyToken<T>(this ModelBuilder modelBuilder) where T : class, IEntity, new()
        //{
        //    modelBuilder.Entity<T>(builder =>
        //    {
        //        builder.Property(entity => entity.RowVersion)
        //            .ValueGeneratedOnAddOrUpdate()
        //            .IsConcurrencyToken();
        //    });
        //}

        //public void DisableCascadeDelete()
        //{
        //    foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
        //    {
        //        relation.DeleteBehavior = DeleteBehavior.Restrict;
        //    }
        //}
    }
}
