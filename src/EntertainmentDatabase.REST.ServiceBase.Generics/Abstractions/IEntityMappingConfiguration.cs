using Microsoft.EntityFrameworkCore;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions
{
    public interface IEntityMappingConfiguration
    {
        void Configure(ModelBuilder builder);
    }
}
