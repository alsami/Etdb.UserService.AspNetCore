using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Misc.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string message) : base(message){}
    }
}
