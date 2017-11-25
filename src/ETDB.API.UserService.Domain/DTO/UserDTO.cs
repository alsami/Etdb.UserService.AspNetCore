using System;
using ETDB.API.UserService.Domain.DTO.Base;

namespace ETDB.API.UserService.Domain.DTO
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
