using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Misc.Exceptions
{
    public class RegisterException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors
        {
            get;
            set;
        }
    }
}
