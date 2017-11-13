using System;

namespace EntertainmentDatabase.REST.API.ServiceBase.Generics.Base
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
