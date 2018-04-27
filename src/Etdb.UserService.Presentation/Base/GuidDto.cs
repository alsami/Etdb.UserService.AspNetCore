using System;

namespace Etdb.UserService.Presentation.Base
{
    public abstract class GuidDto : IBaseDto<Guid>
    {
        public Guid Id { get; set; }
    }
}