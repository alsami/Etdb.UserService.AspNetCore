using System;

namespace EntertainmentDatabase.REST.API.Admin.Base
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
