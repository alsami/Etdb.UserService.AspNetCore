using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserProfileImageUrlFactory
    {
        string GenerateUrl(Guid userId, UserProfileImage profileImage);
    }
}