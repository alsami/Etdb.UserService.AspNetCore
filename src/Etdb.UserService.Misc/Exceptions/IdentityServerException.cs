using System;

namespace Etdb.UserService.Misc.Exceptions
{
    public class IdentityServerException : Exception
    {
        public IdentityServerException(string message) : base(message)
        {
        }
    }
}