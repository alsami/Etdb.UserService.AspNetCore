using Etdb.ServiceBase.Exceptions;

namespace Etdb.UserService.Cqrs
{
    internal static class WellknownExceptions
    {
        public static ResourceNotFoundException UserNotFoundException()
        {
            return new ResourceNotFoundException("Requested user was not found!");
        }
    }
}