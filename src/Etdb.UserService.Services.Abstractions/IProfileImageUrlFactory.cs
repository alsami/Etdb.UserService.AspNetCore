using System;
using Etdb.UserService.Domain.ValueObjects;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IProfileImageUrlFactory
    {
        string GenerateUrl(ProfileImage profileImage, Guid userId);

        string GetResizeUrl(ProfileImage profileImage, Guid userId);

        string GetDeleteUrl(ProfileImage profileImage, Guid userId);
    }
}