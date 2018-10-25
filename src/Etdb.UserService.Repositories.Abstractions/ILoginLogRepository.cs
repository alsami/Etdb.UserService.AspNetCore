using System;
using System.Collections.Generic;
using System.Text;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ILoginLogRepository : IDocumentRepository<LoginLog, Guid>
    {
    }
}
