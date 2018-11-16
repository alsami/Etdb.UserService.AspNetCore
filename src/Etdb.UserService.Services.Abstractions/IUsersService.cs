using System;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUsersService
    {
        Task AddAsync(User user);

        Task<bool> EditAsync(User user);

        Task<User> FindByIdAsync(Guid id);

        Task<User> FindByUserNameAsync(string userName);

        Task<User> FindByUserNameOrEmailAsync(string userNameOrEmail);

        Email FindEmailAddress(string emailAddress);
    }
}