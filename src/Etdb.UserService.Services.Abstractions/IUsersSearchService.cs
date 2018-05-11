using System;
using System.Threading.Tasks;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUsersSearchService
    {        
        Task<User> FindUserByIdAsync(Guid id);

        Task<User> FindUserByUserNameAsync(string userName);

        Task<User> FindUserByUserNameOrEmailAsync(string userNameOrEmail);

        Task<Email> FindEmailAddress(string emailAddress);
    }
}