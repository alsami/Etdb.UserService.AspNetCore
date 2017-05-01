using System;
using EntertainmentDatabase.REST.API.Presentation.Base;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects
{
    public class MovieFileDTO : IDataTransferObject
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

        public string Href
        {
            get;
            set;
        }

        public string File
        {
            get;
            set;
        }
    }
}
