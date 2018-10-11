using System;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserProfileImageUrlFactory
    {
        string GenerateUrl(Guid userId, UserProfileImage profileImage);
    }
}