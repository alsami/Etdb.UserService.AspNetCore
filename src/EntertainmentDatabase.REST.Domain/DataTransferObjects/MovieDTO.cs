using System;
using EntertainmentDatabase.REST.Domain.Abstractions;

namespace EntertainmentDatabase.REST.Domain.DataTransferObjects
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
    