using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation.Results;

namespace Etdb.UserService.EventSourcing.Abstractions.Validation
{
    public abstract class UserCommandValidation<TTransactionCommand, TResponse> : UserValidationDefinition<TTransactionCommand, TResponse>
        where TTransactionCommand : UserCommand<TResponse>
        where TResponse : class
    {
        protected UserCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.RegisterUserNameRule();
            this.RegisterEmailRule();
        }

        public override bool IsValid(TTransactionCommand sourcingCommand, out ValidationResult validationResult)
        {
            validationResult = this.Validate(sourcingCommand);
            return validationResult.IsValid;
        }
    }
}
