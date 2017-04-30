using System;

namespace EntertainmentDatabase.REST.API.Admin.DataTransferObjects
{
    public class ActorDTO : Domain.Base.IDataTransferObject
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
