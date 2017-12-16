using System;
using System.Collections.Generic;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.DTO.Base;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Base;

namespace Etdb.UserService.Presentation.DTO
{
    public class UserDTO : IDataTransferObject, IEventSourcingDTO
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
