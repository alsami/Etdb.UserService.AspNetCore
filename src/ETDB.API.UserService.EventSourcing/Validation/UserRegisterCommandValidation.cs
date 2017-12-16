using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation.Results;

namespace Etdb.UserService.EventSourcing.Validation
{
    public class UserRegisterCommandValidation : UserCommandValidation
    {
        public UserRegisterCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.ValidateUserNameAndEmailNotTaken();
        }

        public override bool IsValid(UserCommand sourcingCommand, out ValidationResult validationResult)
        {
            validationResult = this.Validate(sourcingCommand);
            return validationResult.IsValid;
        }
    }
}
