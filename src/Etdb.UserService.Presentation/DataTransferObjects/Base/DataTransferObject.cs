using System;
using System.Collections.Generic;
using System.Text;

namespace Etdb.UserService.Presentation.DataTransferObjects.Base
{
    public abstract class DataTransferObject : IDataTransferObject
    {
        public Guid Id { get; set; }
        public byte[] ConccurencyToken { get; set; }
    }
}
