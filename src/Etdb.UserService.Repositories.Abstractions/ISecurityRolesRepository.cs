using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ISecurityRolesRepository
    {
        Task<SecurityRole> FindAsync(Guid id);

        Task<SecurityRole> FindAsync(Expression<Func<SecurityRole, bool>> predicate);
    }
}