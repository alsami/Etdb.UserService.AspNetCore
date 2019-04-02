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
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Etdb.UserService.Services.Abstractions.Models;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
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
        private readonly IFileService fileService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;

        public UserRegisterCommandHandler(IUsersService usersService,
            ICommandValidation<UserRegisterCommand> userRegisterCommandValidation,
            ICommandValidation<EmailAddCommand> emailAddCommandValidation,
            ICommandValidation<PasswordAddCommand> passwordCommandValidation,
            ISecurityRolesRepository rolesRepository, IHasher hasher, IMapper mapper, IOptions<FilestoreConfiguration> fileStoreOptions, IFileService fileService)
        {
            this.usersService = usersService;
            this.userRegisterCommandValidation = userRegisterCommandValidation;
            this.emailAddCommandValidation = emailAddCommandValidation;
            this.passwordCommandValidation = passwordCommandValidation;
            this.rolesRepository = rolesRepository;
            this.hasher = hasher;
            this.mapper = mapper;
            this.fileStoreOptions = fileStoreOptions;
            this.fileService = fileService;
        }

        public async Task<UserDto> Handle(UserRegisterCommand command, CancellationToken cancellationToken)
        {
            var provider = (AuthenticationProvider) Enum.Parse(typeof(AuthenticationProvider),
                command.LoginProvider.ToString(), true);

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

            var (user, profileImageMetaInfo) = await this.GenerateUserAsync(command, provider);

            var profileImageIsNull = profileImageMetaInfo == null;
            
            if (!profileImageIsNull) user.AddProfileImage(profileImageMetaInfo.ProfileImage);

            await this.usersService.AddAsync(user, 
                !profileImageIsNull ? new[]
            {
                profileImageMetaInfo
            } : null);

            return this.mapper.Map<UserDto>(user);
        }

        private async Task<(User, ProfileImageMetaInfo)> GenerateUserAsync(UserRegisterCommand command, AuthenticationProvider provider)
        {
            var emails = GenerateEmails(command, provider);

            var roles = await this.GenerateRoleIdsAsync();

            var profileImageMetaInfo = GenerateProfileImageMetaInfoWhenAvailable(command);

            if (provider != AuthenticationProvider.UsernamePassword)
            {
                var userFromExternalAuthentication = new User(command.Id, command.UserName, command.FirstName, command.Name, null,
                    DateTime.UtcNow, roles, emails, provider);

                return (userFromExternalAuthentication, profileImageMetaInfo);
            }

            var salt = this.hasher.GenerateSalt();

            var userFromInternalAuthentication = new User(command.Id, command.UserName, command.FirstName, command.Name, null,
                DateTime.UtcNow, roles, emails,
                password: this.hasher.CreateSaltedHash(command.PasswordAddCommand.NewPassword, salt), salt: salt);
            
            return (userFromInternalAuthentication, profileImageMetaInfo);
        }

        private static ProfileImageMetaInfo GenerateProfileImageMetaInfoWhenAvailable(UserRegisterCommand command)
        {
            if (command.ProfileImageAddCommand == null) return null;
            
            return new ProfileImageMetaInfo(ProfileImage.Create(command.Id,
                    command.ProfileImageAddCommand.FileName,
                    command.ProfileImageAddCommand.FileContentType.MediaType,
                    command.ProfileImageAddCommand.IsPrimary),
                command.ProfileImageAddCommand.FileBytes);
        }
        
        private static ICollection<Email> GenerateEmails(UserRegisterCommand command, AuthenticationProvider provider)
            => command
                .Emails
                .Select(email => new Email(email.Id, command.Id, email.Address, email.IsPrimary,
                    provider != AuthenticationProvider.UsernamePassword))
                .ToArray();

        private async Task<ICollection<Guid>> GenerateRoleIdsAsync()
        {
            var memberRole = await this.rolesRepository.FindAsync(role => role.Name == RoleNames.Member);
            
            if (memberRole == null) throw new ApplicationException("Required minimum data not available!");

            return new[]
            {
                memberRole.Id
            };
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
    }
}