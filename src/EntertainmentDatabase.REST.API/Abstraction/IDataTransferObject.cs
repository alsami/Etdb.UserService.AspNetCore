using System;

namespace EntertainmentDatabase.REST.API.Abstraction
{
    public interface IDataTransferObject
    {
        Guid Id { get; set; }

        byte[] ConcurrencyToken { get; set; }
    }
}
