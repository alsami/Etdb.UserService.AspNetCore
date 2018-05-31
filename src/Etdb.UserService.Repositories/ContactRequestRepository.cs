using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class ContactRequestRepository : GenericDocumentRepository<ContactRequest, Guid>, IContactRequestRepository
    {
        public ContactRequestRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}