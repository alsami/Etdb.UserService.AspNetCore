using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.ServiceBase.DataAccess.Abstraction;
using EntertainmentDatabase.REST.ServiceBase.DataAccess.Extensions;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.DataAccess.Facades
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class, IEntity, new()
    {
        public abstract void Map(EntityTypeBuilder<T> builder);

        public void Map(ModelBuilder builder)
        {
            builder.SetGuidAsPrimaryKey<T>();
            this.Map(builder.Entity<T>());
        }
    }
}
