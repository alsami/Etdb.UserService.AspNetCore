using System;

namespace ETDB.API.UserService.Presentation.Base
{
    public interface IDataTransferObject
    {
        Guid Id
        {
            get;
            set;
        }

        byte[] ConccurencyToken
        {
            get;
            set;
        }
    }
}
