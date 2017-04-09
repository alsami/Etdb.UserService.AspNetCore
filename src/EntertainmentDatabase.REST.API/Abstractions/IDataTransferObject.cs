using System;

namespace EntertainmentDatabase.REST.API.Abstractions.Base
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
