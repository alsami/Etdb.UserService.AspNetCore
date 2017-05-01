using System;

namespace EntertainmentDatabase.REST.API.Presentation.Base
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
