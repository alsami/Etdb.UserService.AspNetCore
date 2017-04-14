using System;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using EntertainmentDatabase.REST.ServiceBase.Generics.Enums;

namespace EntertainmentDatabase.REST.Domain.Entities
{
    public class MovieFile : IMediaEntity
    {
        public Guid MovieId
        {
            get;
            set;
        }

        public Movie Movie
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Extension
        {
            get;
            set;
        }

        public Guid UniqueName
        {
            get;
            set;
        }

        public MediaType MediaType
        {
            get;
            set;
        }

        public byte[] File
        {
            get;
            set;
        }

        public bool IsCover
        {
            get;
            set;
        }
    }
}
