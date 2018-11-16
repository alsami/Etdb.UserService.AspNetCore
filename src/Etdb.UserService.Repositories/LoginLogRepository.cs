using System;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class LoginLogRepository : GenericDocumentRepository<LoginLog, Guid>, ILoginLogRepository
    {
        public LoginLogRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}