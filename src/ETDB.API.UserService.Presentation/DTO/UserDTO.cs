using System;
using System.Collections;
using System.Collections.Generic;
using ETDB.API.UserService.Presentation.DTO.Base;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Presentation.DTO
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

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
