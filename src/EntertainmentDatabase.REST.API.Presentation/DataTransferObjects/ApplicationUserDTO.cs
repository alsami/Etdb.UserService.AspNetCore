using System;
using EntertainmentDatabase.REST.API.Presentation.Base;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects
{
    public class ApplicationUserDTO : IDataTransferObject
    {
        public Guid Id
        {
            get;
            set;
        }

        public byte[] ConcurrencyToken
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }
}
