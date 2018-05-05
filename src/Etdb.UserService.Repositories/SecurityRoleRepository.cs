﻿using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class SecurityRoleRepository : GenericDocumentRepository<SecurityRole, Guid>, ISecurityRolesRepository
    {
        public SecurityRoleRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}