using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Constants;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using FluentValidation.Results;
using MediatR;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserRegisterCommandHandler : IVoidCommandHandler<UserRegisterCommand>
    {
        private readonly IUsersService usersService;
        private readonly ICommandValidation<UserRegisterCommand> userRegisterCommandValidation;
        private readonly ICommandValidation<EmailAddCommand> emailAddCommandValidation;
        private readonly ICommandValidation<PasswordAddCommand> passwordCommandValidation;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IHasher hasher;

        public UserRegisterCommandHandler(IUsersService usersService,
            ICommandValidation<UserRegisterCommand> userRegisterCommandValidation,
            ICommandValidation<EmailAddCommand> emailAddCommandValidation,
            ICommandValidation<PasswordAddCommand> passwordCommandValidation,
            ISecurityRolesRepository rolesRepository, IHasher hasher)
        {
            this.usersService = usersService;
            this.userRegisterCommandValidation = userRegisterCommandValidation;
            this.emailAddCommandValidation = emailAddCommandValidation;
            this.passwordCommandValidation = passwordCommandValidation;
            this.rolesRepository = rolesRepository;
            this.hasher = hasher;
        }

        public async Task<Unit> Handle(UserRegisterCommand command, CancellationToken cancellationToken)
        {
            var validationResults = await this.ValidateRequestAsync(command);

            if (validationResults.Any(result => !result.IsValid))
            {
                var errors = validationResults
                    .Where(result => !result.IsValid)
                    .SelectMany(result => result.Errors)
                    .Select(result => result.ErrorMessage)
                    .ToArray();

                throw new GeneralValidationException("Error validating user registration!", errors);
            }

            var user = await this.GenerateUserAsync(command);

            await this.usersService.AddAsync(user);

            return Unit.Value;
        }

        private async Task<ICollection<ValidationResult>> ValidateRequestAsync(UserRegisterCommand command)
        {
            var validationTasks = command.Emails
                .Select(async emailAddCommand =>
                    await this.emailAddCommandValidation.ValidateCommandAsync(emailAddCommand))
                .ToList();

            validationTasks.Add(this.userRegisterCommandValidation.ValidateCommandAsync(command));

            validationTasks.Add(this.passwordCommandValidation.ValidateCommandAsync(command.PasswordAddCommand));

            return await Task.WhenAll(validationTasks);
        }

        private async Task<User> GenerateUserAsync(UserRegisterCommand command)
        {
            var emails = command
                .Emails
                .Select(email => new Email(Guid.NewGuid(), email.Address, email.IsPrimary))
                .ToArray();

            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);

            var salt = this.hasher.GenerateSalt();

            return new User(Guid.NewGuid(), command.UserName, command.FirstName, command.Name, null,
                this.hasher.CreateSaltedHash(command.PasswordAddCommand.NewPassword, salt), salt, DateTime.UtcNow, null,
                new[] {memberRole.Id}, emails);
        }
    }
}