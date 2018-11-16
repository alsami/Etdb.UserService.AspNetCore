using System;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
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