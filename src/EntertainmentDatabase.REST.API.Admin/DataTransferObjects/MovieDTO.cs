using System;

namespace EntertainmentDatabase.REST.API.Admin.DataTransferObjects
{
    public class MovieDTO : Domain.Base.IDataTransferObject
    {
        public byte[] ConcurrencyToken
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public DateTime? ReleasesOn
        {
            get;
            set;
        }
    }
}
    