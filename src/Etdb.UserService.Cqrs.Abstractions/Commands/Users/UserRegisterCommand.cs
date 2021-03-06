﻿using System;
using System.Collections.Generic;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Cqrs.Abstractions.Commands.Emails;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserRegisterCommand : UserNameCommand, IRequest<UserDto>
    {
        public UserRegisterCommand(Guid id, string wantedUserName, string firstName, string name,
            ICollection<EmailAddCommand>? emails,
            int loginProvider, PasswordAddCommand? passwordAddCommand = null,
            ProfileImageAddCommand? profileImageAddCommand = null) : base(
            id, wantedUserName)
        {
            this.FirstName = firstName;
            this.Name = name;
            this.Emails = emails;
            this.LoginProvider = loginProvider;
            this.PasswordAddCommand = passwordAddCommand;
            this.ProfileImageAddCommand = profileImageAddCommand;
        }

        public string FirstName { get; }

        public string Name { get; }

        public ICollection<EmailAddCommand>? Emails { get; }

        public int LoginProvider { get; }

        public PasswordAddCommand? PasswordAddCommand { get; }
        public ProfileImageAddCommand? ProfileImageAddCommand { get; }
    }
}