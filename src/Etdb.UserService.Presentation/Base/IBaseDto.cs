using System;

namespace Etdb.UserService.Presentation.Base
{
    public interface IBaseDto<TId> where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}