using Etdb.UserService.EventSourcing.Abstractions.Validation;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.Validation
{
    public class UserUpdateCommandValidation : UserCommandValidation<UserUpdateCommand, UserDto>
    {
        public UserUpdateCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.RegisterUserNameAndEmailNotTakenRule();
        }
    }
}
