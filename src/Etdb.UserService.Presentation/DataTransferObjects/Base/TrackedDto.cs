using System;

namespace Etdb.UserService.Presentation.DataTransferObjects.Base
{
    public abstract class TrackedDto : DataTransferObject
    {
        public string CreateUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
