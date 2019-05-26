using System;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions.Models;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUsersService
    {
        Task AddAsync(User user, params ProfileImageMetaInfo[] profileImageMetaInfos);

        Task EditAsync(User user);

        Task EditAsync(User user, params ProfileImageMetaInfo[] profileImageMetaInfos);

        Task<User> FindByIdAsync(Guid id);

        Task<bool> IsUserLocked(Guid id);

        Task<User> FindByUserNameAsync(string userName);

        Task<User> FindByUserNameOrEmailAsync(string userNameOrEmail);

        Email FindEmailAddress(string emailAddress);
    }
}