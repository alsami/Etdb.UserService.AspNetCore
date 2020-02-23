using System;

namespace Etdb.UserService.Presentation.Users
{
    public class UserFlatDto
    {
        public Guid Guid { get; set; }

        public string UserName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime RegisteredAt { get; set; }

        public UserFlatDto()
        {
        }

        public UserFlatDto(Guid guid, string userName, string? imageUrl, DateTime registeredAt)
        {
            this.Guid = guid;
            this.UserName = userName;
            this.ImageUrl = imageUrl;
            this.RegisteredAt = registeredAt;
        }
    }
}