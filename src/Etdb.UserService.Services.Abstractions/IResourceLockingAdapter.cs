using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IResourceLockingAdapter
    {
        Task<bool> LockAsync(object key, TimeSpan lockSpan);

        Task UnlockAsync(object key);
    }
}