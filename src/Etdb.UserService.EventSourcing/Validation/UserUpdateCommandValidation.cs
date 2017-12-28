using Etdb.UserService.EventSourcing.Abstractions.Validation;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.Validation
{
    public class UserUpdateCommandValidation : UserCommandValidation<UserUpdateCommand, UserDTO>
    {
        public UserUpdateCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.RegisterUserNameAndEmailNotTakenRule();
        }
    }
}
