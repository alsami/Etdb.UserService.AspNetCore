using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);

        Task<bool> EditAsync(User user);

        Task<User?> FindAsync(Guid id);

        Task<User?> FindAsync(Expression<Func<User, bool>> predicate);

        Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate);

        Email? FindEmailAddress(string emailAddress);
    }
}