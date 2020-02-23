using System;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;

namespace Etdb.UserService.Services.Abstractions.Models
{
    public class StorableImage
    {
        public StorableImage(Guid userId, ProfileImage profileImage, ReadOnlyMemory<byte> image)
        {
            this.UserId = userId;
            this.ProfileImage = profileImage;
            this.Image = image;
        }

        public Guid UserId { get; }

        public ProfileImage ProfileImage { get; }

        public ReadOnlyMemory<byte> Image { get; }
    }
} 