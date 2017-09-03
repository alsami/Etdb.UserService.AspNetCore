using System;
using EntertainmentDatabase.REST.API.Presentation.Base;
using EntertainmentDatabase.REST.API.Domain.Enums;

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

        public ConsumerMediaType ConsumerMediaType
        {
            get;
            set;
        }

        public DateTime? ReleasedOn
        {
            get;
            set;
        }
    }
}
    