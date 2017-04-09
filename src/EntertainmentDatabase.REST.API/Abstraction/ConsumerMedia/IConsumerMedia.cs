using System;
using EntertainmentDatabase.REST.API.Enums;

namespace EntertainmentDatabase.REST.API.Abstraction.ConsumerMedia
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
