using System;
using ETDB.API.UserService.Presentation.Base;

namespace ETDB.API.UserService.Presentation.DataTransferObjects
{
    public class UserDTO : IDataTransferObject
    {
        public Guid Id
        {
            get;
            set;
        }

        public byte[] ConccurencyToken
        {
            get;
            set;
        }


        public string Name
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }
    }
}
