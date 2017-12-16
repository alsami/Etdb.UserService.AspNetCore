using System;
using Etdb.ServiceBase.EventSourcing.Validation;
using Etdb.UserService.EventSourcing.Abstractions.Commands;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation;

namespace Etdb.UserService.EventSourcing.Abstractions.Validation
{
    public abstract class UserValidationDefinition<TTransactionCommand, TResponse> : CommandValidation<TTransactionCommand, TResponse>
        where TTransactionCommand: UserCommand<TResponse>
        where TResponse : class
    {
        private readonly IUserRepository userRepository;

        protected UserValidationDefinition(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        protected void RegisterUserNameAndEmailNotTakenRule()
        {
            this.RuleFor(user => user)
                .Must(this.IsUnique)
                .WithMessage("Username or email addresss already taken!");
        }

        protected void RegisterUserNameRule()
        {
            this.RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("Username may not be empty!")
                .NotNull()
                .WithMessage("Username may not be null!")
                .NotEqual("Admin", StringComparer.OrdinalIgnoreCase)
                .NotEqual("Administrator", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Username blacklisted!")
                .MinimumLength(6)
                .WithMessage("Username must have at least six letters!")
                .MaximumLength(32)
                .WithMessage("Username may not be longer than 32 characters");
        }

        protected void RegisterEmailRule()
        {
            this.RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("Email address may not be empty!")
                .NotNull()
                .WithMessage("Email may not be null!")
                .EmailAddress()
                .WithMessage("Email must be valid!");
        }

        protected void RegisterNameRule()
        {
            this.RuleFor(user => user.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name must be given!");
        }

        protected void RegisterLastNameRule()
        {
            this.RuleFor(user => user.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lastname must be given!");
        }

        protected void RegisterPasswordRule()
        {
            this.RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Password must be given!")
                .Length(8, 64)
                .WithMessage("Password must have 8 to 64 characters");
        }

        protected void RegisterRowVersionRule()
        {
            this.RuleFor(user => user.RowVersion)
                .NotEmpty()
                .NotNull()
                .WithMessage("Concurrency token must be given!");
        }

        private bool IsUnique(UserCommand<TResponse> command)
        {
            var existingUser = this.userRepository
                .Find(command.UserName, command.Email);

            if (existingUser == null)
            {
                return true;
            }

            return existingUser.Id == command.Id;
        }
    }
}
