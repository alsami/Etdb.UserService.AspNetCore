using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Events;

namespace Etdb.UserService.Application.Automapper.Converter
{
    public class UserRegisterEventConverter : ITypeConverter<User, UserRegisterEvent>
    {
        public UserRegisterEvent Convert(User source, UserRegisterEvent destination, ResolutionContext context)
        {
            return new UserRegisterEvent(source.Id, source.Name, source.LastName, source.Email, source.UserName);
        }
    }
}
