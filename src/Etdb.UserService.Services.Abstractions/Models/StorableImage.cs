using System;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions.Models
{
    public class StorableImage
    {
        public StorableImage(ProfileImage profileImage, ReadOnlyMemory<byte> image)
        {
            this.ProfileImage = profileImage;
            this.Image = image;
        }

        public ProfileImage ProfileImage { get; }

        public ReadOnlyMemory<byte> Image { get; }
    }
} 