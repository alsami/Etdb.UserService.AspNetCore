using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions;
using Etdb.UserService.Constants;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using FluentValidation.Results;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserRegisterCommandHandler : IVoidCommandHandler<UserRegisterCommand>
    {
        private readonly IAuthService authService;
        private readonly ICommandValidation<UserRegisterCommand> userRegisterCommandValidation;
        private readonly ICommandValidation<EmailAddCommand> emailAddCommandValidation;
        private readonly ICommandValidation<PasswordAddCommand> passwordCommandValidation;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IHasher hasher;

        public UserRegisterCommandHandler(IAuthService authService,
            ICommandValidation<UserRegisterCommand> userRegisterCommandValidation,
            ICommandValidation<EmailAddCommand> emailAddCommandValidation,
            ICommandValidation<PasswordAddCommand> passwordCommandValidation, 
            ISecurityRolesRepository rolesRepository, IHasher hasher)
        {
            
            this.authService = authService;
            this.userRegisterCommandValidation = userRegisterCommandValidation;
            this.emailAddCommandValidation = emailAddCommandValidation;
            this.passwordCommandValidation = passwordCommandValidation;
            this.rolesRepository = rolesRepository;
            this.hasher = hasher;
        }

        public async Task Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResults =
                new List<ValidationResult>
                {
                    await this.userRegisterCommandValidation.ValidateCommandAsync(request)
                };

            foreach (var emailAddCommand in request.Emails)
            {
                validationResults.Add(await this.emailAddCommandValidation.ValidateCommandAsync(emailAddCommand));
            }

            var passwordAddCommand = new PasswordAddCommand
            {
                Password = request.Password
            };
            
            validationResults.Add(await this.passwordCommandValidation.ValidateCommandAsync(passwordAddCommand));

            if (validationResults.Any(result => !result.IsValid))
            {
                var errors = validationResults
                    .Where(result => !result.IsValid)
                    .SelectMany(result => result.Errors)
                    .Select(result => result.ErrorMessage)
                    .ToArray();
                
                throw new GeneralValidationException("Error validating user registration!", errors);
            }

            var user = await this.GenerateUser(request);

            await this.authService.RegisterAsync(user);
        }

        private async Task<User> GenerateUser(UserRegisterCommand command)
        {
            var emails = command
                .Emails
                .Select(email => new Email(Guid.NewGuid(), email.Address, email.IsPrimary))
                .ToArray();
            
            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);

            var salt = this.hasher.GenerateSalt();
            
            return new User(Guid.NewGuid(), command.UserName, command.FirstName, command.Name, salt,
                this.hasher.CreateSaltedHash(command.Password, salt), new[] { memberRole.Id }, emails);
        }
    }
}