using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserUpdateEventConverter : ITypeConverter<User, UserUpdateEvent>
    {
        public UserUpdateEvent Convert(User source, UserUpdateEvent destination, ResolutionContext context)
        {
            return new UserUpdateEvent(source.Id, source.Name, source.LastName, source.Email, source.UserName);
        }
    }
}
