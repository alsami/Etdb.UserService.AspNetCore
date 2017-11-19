using System;
using System.Collections.Generic;
using System.Text;

namespace ETDB.API.UserService.Presentation.DataTransferObjects
{
    public class WebClientConfigDTO
    {
        public string ClientName
        {
            get;
            set;
        }

        public string Secret
        {
            get;
            set;
        }
    }
}
