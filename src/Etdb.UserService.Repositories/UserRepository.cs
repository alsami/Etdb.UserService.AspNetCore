using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class UserRepository : GenericDocumentRepository<User, Guid>, IUserRepository
    {
        private readonly DocumentDbContext context;

        public UserRepository(DocumentDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}