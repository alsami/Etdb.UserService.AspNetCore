using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.UserService.Domain.DTO;

namespace ETDB.API.UserService.Domain
{
    public interface IUserAppService
    {
        void Register(RegisterUserDTO registerUserDTO);
    }
}
