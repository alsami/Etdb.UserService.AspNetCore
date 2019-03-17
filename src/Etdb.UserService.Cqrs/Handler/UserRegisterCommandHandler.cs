using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Constants;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using FluentValidation.Results;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserRegisterCommandHandler : IResponseCommandHandler<UserRegisterCommand, UserDto>
    {
        private readonly IUsersService usersService;
        private readonly ICommandValidation<UserRegisterCommand> userRegisterCommandValidation;
        private readonly ICommandValidation<EmailAddCommand> emailAddCommandValidation;
        private readonly ICommandValidation<PasswordAddCommand> passwordCommandValidation;
        private readonly ISecurityRolesRepository rolesRepository;
        private readonly IHasher hasher;
        private readonly IMapper mapper;

        public UserRegisterCommandHandler(IUsersService usersService,
            ICommandValidation<UserRegisterCommand> userRegisterCommandValidation,
            ICommandValidation<EmailAddCommand> emailAddCommandValidation,
            ICommandValidation<PasswordAddCommand> passwordCommandValidation,
            ISecurityRolesRepository rolesRepository, IHasher hasher, IMapper mapper)
        {
            this.usersService = usersService;
            this.userRegisterCommandValidation = userRegisterCommandValidation;
            this.emailAddCommandValidation = emailAddCommandValidation;
            this.passwordCommandValidation = passwordCommandValidation;
            this.rolesRepository = rolesRepository;
            this.hasher = hasher;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(UserRegisterCommand command, CancellationToken cancellationToken)
        {
            var provider = (AuthenticationProvider) Enum.Parse(typeof(AuthenticationProvider), command.LoginProvider.ToString(), true);

            var validationResults = await this.ValidateRequestAsync(command, provider);

            if (validationResults.Any(result => !result.IsValid))
            {
                var errors = validationResults
                    .Where(result => !result.IsValid)
                    .SelectMany(result => result.Errors)
                    .Select(result => result.ErrorMessage)
                    .ToArray();

                throw new GeneralValidationException("Error validating user registration!", errors);
            }

            var user = await this.GenerateUserAsync(command, provider);

            await this.usersService.AddAsync(user);

            if (command.ProfileImageAddCommand == null)
            {
                return this.mapper.Map<UserDto>(user);
            }

            var profileImage = UserProfileImage.Create(user.Id, command.ProfileImageAddCommand.FileName,
                command.ProfileImageAddCommand.FileContentType.MediaType);

            var userWithImage = await this.usersService.EditProfileImageAsync(user, profileImage,
                command.ProfileImageAddCommand.FileBytes);

            return this.mapper.Map<UserDto>(userWithImage);
        }

        private async Task<ICollection<ValidationResult>> ValidateRequestAsync(UserRegisterCommand command,
            AuthenticationProvider provider)
        {
            var validationTasks = command.Emails
                .Select(async emailAddCommand =>
                    await this.emailAddCommandValidation.ValidateCommandAsync(emailAddCommand))
                .ToList();

            validationTasks.Add(this.userRegisterCommandValidation.ValidateCommandAsync(command));

            if (provider == AuthenticationProvider.UsernamePassword)
            {
                validationTasks.Add(this.passwordCommandValidation.ValidateCommandAsync(command.PasswordAddCommand));
            }

            return await Task.WhenAll(validationTasks);
        }

        private async Task<User> GenerateUserAsync(UserRegisterCommand command, AuthenticationProvider provider)
        {
            var emails = command
                .Emails
                .Select(email => new Email(Guid.NewGuid(), email.Address, email.IsPrimary,
                    provider != AuthenticationProvider.UsernamePassword))
                .ToArray();

            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);

            var roles = new[]
            {
                memberRole.Id
            };

            if (provider != AuthenticationProvider.UsernamePassword)
            {
                return new User(Guid.NewGuid(), command.UserName, command.FirstName, command.Name, null,
                    DateTime.UtcNow, roles, emails, provider);
            }

            var salt = this.hasher.GenerateSalt();

            return new User(Guid.NewGuid(), command.UserName, command.FirstName, command.Name, null,
                DateTime.UtcNow, roles, emails,
                password: this.hasher.CreateSaltedHash(command.PasswordAddCommand.NewPassword, salt), salt: salt);
        }
    }
}