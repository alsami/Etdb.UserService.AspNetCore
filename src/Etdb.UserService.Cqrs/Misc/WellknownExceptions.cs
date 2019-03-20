using System;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Cqrs.Misc
{
    internal static class WellknownExceptions
    {
        public static ResourceNotFoundException UserNotFoundException() =>
            new ResourceNotFoundException("Requested user was not found!");

        public static ResourceLockedException UserResourceLockException(Guid id) =>
            new ResourceLockedException(typeof(User), id, "User cannot be changed currently! Please try again later.");
    }
}