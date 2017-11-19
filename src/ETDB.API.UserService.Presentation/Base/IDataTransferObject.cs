using System;
using System.Collections.Generic;
using System.Text;

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
