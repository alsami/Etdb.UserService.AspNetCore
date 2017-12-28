using AutoMapper;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserUpdateCommandConverter : ITypeConverter<UserDTO, UserUpdateCommand>
    {
        public UserUpdateCommand Convert(UserDTO source, UserUpdateCommand destination, ResolutionContext context)
        {
            return new UserUpdateCommand(source.Id, source.ConccurencyToken, source.Name, source.LastName, source.Email, source.UserName);
        }
    }
}
