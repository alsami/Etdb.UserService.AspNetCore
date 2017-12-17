using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserRegisterCommandConverter : ITypeConverter<UserRegisterDTO, UserRegisterCommand>
    {
        public UserRegisterCommand Convert(UserRegisterDTO source, UserRegisterCommand destination, ResolutionContext context)
        {
            return new UserRegisterCommand(source.Name, source.LastName, source.Email, source.UserName, source.Password);
        }
    }
}
