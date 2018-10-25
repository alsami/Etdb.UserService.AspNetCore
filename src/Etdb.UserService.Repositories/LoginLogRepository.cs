using System;
using System.Collections.Generic;
using System.Text;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain.Documents;
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