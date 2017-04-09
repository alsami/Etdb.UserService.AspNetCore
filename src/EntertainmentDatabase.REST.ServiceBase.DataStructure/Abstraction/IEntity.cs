using System;

namespace EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction
{
    public interface IEntity
    {
        Guid Id { get; }

        byte[] RowVersion { get; set; }
    }
}
