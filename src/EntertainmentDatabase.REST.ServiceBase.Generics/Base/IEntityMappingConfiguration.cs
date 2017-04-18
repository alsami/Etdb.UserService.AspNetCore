using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Base
{
    public interface IEntityMappingConfiguration
    {
        void Configure(ModelBuilder builder);
    }
}
