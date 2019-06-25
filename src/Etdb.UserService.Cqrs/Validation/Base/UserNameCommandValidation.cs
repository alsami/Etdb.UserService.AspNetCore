﻿using System;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class UserNameCommandValidation<TUserNameCommand> : CommandValidation<TUserNameCommand>
        where TUserNameCommand : UserNameCommand
    {
        private readonly IUsersService usersService;

        protected UserNameCommandValidation(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        protected void RegisterUserNameRules()
        {
            this.RuleFor(command => command.WantedUserName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username must be given!")
                .MinimumLength(4)
                .MaximumLength(32)
                .WithMessage("User name must contain at least four characters and has a limit of 32 characters!")
                .NotEqual("Administrator", StringComparer.OrdinalIgnoreCase)
                .NotEqual("Admin", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Username blacklisted!")
                .MustAsync(async (command, userName, token) => await this.IsUserNameAvailable(command))
                .WithMessage("The username is already in use!");
        }

        private async Task<bool> IsUserNameAvailable(UserNameCommand command)
        {
            var user = await this.usersService.FindByUserNameAsync(command.WantedUserName);

            return user == null || user.Id == command.Id;
        }
    }
}