using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.Context.Mappings
{
    public class TestEntityMappingConfiguration : EntityMappingConfiguration<Test>
    {
        protected override void Map(EntityTypeBuilder<Test> builder)
        {
            
        }
    }
}
