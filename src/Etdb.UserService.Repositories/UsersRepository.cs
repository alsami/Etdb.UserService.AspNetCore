using System;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class UsersRepository : GenericDocumentRepository<User, Guid>, IUsersRepository
    {
        private readonly DocumentDbContext context;

        public UsersRepository(DocumentDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}