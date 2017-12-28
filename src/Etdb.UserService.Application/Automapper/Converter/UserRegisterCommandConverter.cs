using AutoMapper;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserRegisterCommandConverter : ITypeConverter<UserRegisterDto, UserRegisterCommand>
    {
        public UserRegisterCommand Convert(UserRegisterDto source, UserRegisterCommand destination, ResolutionContext context)
        {
            return new UserRegisterCommand(source.Name, source.LastName, source.Email, source.UserName, source.Password);
        }
    }
}
