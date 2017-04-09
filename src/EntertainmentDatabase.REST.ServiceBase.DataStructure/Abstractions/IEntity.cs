using System;

namespace EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstractions
{
    public interface IEntity
    {
        Guid Id
        {
            get;
            set;
        }

        byte[] RowVersion
        {
            get;
            set;
        }
    }
}
