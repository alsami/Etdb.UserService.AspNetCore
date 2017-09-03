using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Presentation.DataTransferObjects
{
    public class LoginUserDTO
    {
        public string Email
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
