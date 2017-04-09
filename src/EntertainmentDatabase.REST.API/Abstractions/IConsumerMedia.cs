using System;
using EntertainmentDatabase.REST.API.Enums;

namespace EntertainmentDatabase.REST.API.Abstractions
{
    public interface IConsumerMedia
    {
        ConsumerMediaType ConsumerMediaType
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }

        DateTime? ReleasedOn
        {
            get;
            set;
        }
    }
}
