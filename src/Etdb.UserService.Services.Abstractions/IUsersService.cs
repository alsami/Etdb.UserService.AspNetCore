using System;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUsersService
    {
        Task AddAsync(User user);

        Task<bool> EditUserAsync(User user);

        Task<User> FindUserByIdAsync(Guid id);

        Task<User> FindUserByUserNameAsync(string userName);

        Task<User> FindUserByUserNameOrEmailAsync(string userNameOrEmail);

        Email FindEmailAddress(string emailAddress);
    }
}