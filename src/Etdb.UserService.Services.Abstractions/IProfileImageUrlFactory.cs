using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IProfileImageUrlFactory
    {
        string GenerateUrl(ProfileImage profileImage);

        string GetResizeUrl(ProfileImage profileImage);

        string GetDeleteUrl(ProfileImage profileImage);
    }
}