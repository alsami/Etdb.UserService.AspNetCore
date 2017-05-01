using System;
using EntertainmentDatabase.REST.API.Presentation.Base;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects
{
    public class MovieDTO : IDataTransferObject
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
    