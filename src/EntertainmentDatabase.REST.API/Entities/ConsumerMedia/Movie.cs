using System;
using EntertainmentDatabase.REST.API.Abstractions.ConsumerMedia;
using EntertainmentDatabase.REST.API.Enums;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstractions;

namespace EntertainmentDatabase.REST.API.Entities.ConsumerMedia
{
    public class Movie : IEntity, IConsumerMedia
    {
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

        public string Title
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
