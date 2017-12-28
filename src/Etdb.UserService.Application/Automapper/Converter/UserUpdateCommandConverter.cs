using AutoMapper;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserUpdateCommandConverter : ITypeConverter<UserDto, UserUpdateCommand>
    {
        public UserUpdateCommand Convert(UserDto source, UserUpdateCommand destination, ResolutionContext context)
        {
            return new UserUpdateCommand(source.Id, source.ConccurencyToken, source.Name, source.LastName, source.Email, source.UserName);
        }
    }
}
