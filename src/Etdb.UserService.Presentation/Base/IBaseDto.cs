using System;

namespace Etdb.UserService.Presentation.Base
{
    public interface IBaseDto<out TId> where TId : IEquatable<TId>
    {
        TId Id { get; }
    }
}