using System;

namespace EntertainmentDatabase.REST.Domain.Abstractions
{
    public interface IDataTransferObject
    {
        Guid Id
        {
            get;
            set;
        }

        byte[] ConcurrencyToken
        {
            get;
            set;
        }
    }
}
