using System;
using EntertainmentDatabase.REST.ServiceBase.Generics.Enums;

namespace EntertainmentDatabase.REST.Domain.Abstractions
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
