using Etdb.UserService.EventSourcing.Abstractions.Validation;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation.Results;

namespace Etdb.UserService.EventSourcing.Validation
{
    public class UserRegisterCommandValidation : UserCommandValidation<UserRegisterCommand, UserDTO>
    {
        public UserRegisterCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.RegisterUserNameAndEmailNotTakenRule();
        }

        public override bool IsValid(UserRegisterCommand sourcingCommand, out ValidationResult validationResult)
        {
            validationResult = this.Validate(sourcingCommand);
            return validationResult.IsValid;
        }
    }
}
