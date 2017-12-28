using Etdb.UserService.EventSourcing.Abstractions.Validation;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.Validation
{
    public class UserRegisterCommandValidation : UserCommandValidation<UserRegisterCommand, UserDTO>
    {
        public UserRegisterCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.RegisterPasswordRule();
            this.RegisterUserNameAndEmailNotTakenRule();
        }
    }
}
