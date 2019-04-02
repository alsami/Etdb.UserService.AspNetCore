using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions.Models;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUsersService
    {
        Task AddAsync(User user, ICollection<ProfileImageMetaInfo> profileImageMetaInfos = null);

        Task EditAsync(User user);

        Task<User> EditProfileImageAsync(User user, ProfileImage profileImage, byte[] file);

        Task<User> FindByIdAsync(Guid id);

        Task<bool> IsUserLocked(Guid id);

        Task<User> FindByUserNameAsync(string userName);

        Task<User> FindByUserNameOrEmailAsync(string userNameOrEmail);

        Email FindEmailAddress(string emailAddress);
    }
}