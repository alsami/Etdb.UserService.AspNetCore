using System;

namespace EntertainmentDatabase.REST.API.Abstractions
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
