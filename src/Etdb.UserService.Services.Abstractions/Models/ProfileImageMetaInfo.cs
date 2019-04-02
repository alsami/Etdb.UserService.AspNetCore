using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions.Models
{
    public class ProfileImageMetaInfo
    {
        public ProfileImageMetaInfo(ProfileImage profileImage, byte[] image)
        {
            this.ProfileImage = profileImage;
            this.Image = image;
        }

        public ProfileImage ProfileImage { get; }

        public byte[] Image { get; }
    }
}