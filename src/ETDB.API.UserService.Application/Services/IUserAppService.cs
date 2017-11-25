using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Application.Services
{
    public interface IUserAppService
    {
        void Register(RegisterUserDTO registerUserDTO);
    }
}
