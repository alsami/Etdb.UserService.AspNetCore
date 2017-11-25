using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Hasher;
using ETDB.API.ServiceBase.Hasher;
using ETDB.API.UserService.Domain.Commands;
using ETDB.API.UserService.Domain.DTO;

namespace ETDB.API.UserService.Domain.Mappings
{
    public class RegisterUserDTOMapping : Profile
    {
        public RegisterUserDTOMapping()
        {
            var hasher = new Hasher();
            var salt = hasher.GenerateSalt();
            this.CreateMap<RegisterUserDTO, RegisterUserCommand>()
                .ConstructUsing(registerUser => new RegisterUserCommand(registerUser.Name,
                    registerUser.LastName, registerUser.Email, registerUser.UserName,
                    hasher.CreateSaltedHash(registerUser.Password, salt), salt));
        }
    }
}
