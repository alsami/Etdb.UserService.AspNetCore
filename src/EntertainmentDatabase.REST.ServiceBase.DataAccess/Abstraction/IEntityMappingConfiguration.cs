using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.ServiceBase.DataAccess.Abstraction
{
    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder builder);
    }
}
