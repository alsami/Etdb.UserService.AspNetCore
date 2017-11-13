using System;
using EntertainmentDatabase.REST.API.WebService.Domain.Enums;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Base
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
