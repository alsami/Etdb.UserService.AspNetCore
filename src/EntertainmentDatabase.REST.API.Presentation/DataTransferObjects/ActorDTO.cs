using System;
using EntertainmentDatabase.REST.API.Presentation.Base;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects
{
    public class ActorDTO : IDataTransferObject
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

        public string Name
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }
    }
}
