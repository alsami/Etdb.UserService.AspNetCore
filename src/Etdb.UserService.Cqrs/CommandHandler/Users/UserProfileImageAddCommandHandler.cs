using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    // ReSharper disable once UnusedMember.Global
    public class UserProfileImageAddCommandHandler : IResponseCommandHandler<ProfileImageAddCommand, UserDto>
    {
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IMapper mapper;

        public UserProfileImageAddCommandHandler(IUsersService usersService, IMapper mapper,
            IResourceLockingAdapter resourceLockingAdapter)
        {
            this.usersService = usersService;
            this.mapper = mapper;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<UserDto> Handle(ProfileImageAddCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.UserId);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found");
            }

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30)))
            {
                throw WellknownExceptions.UserResourceLockException(existingUser.Id);
            }

            var userProfileImage =
                ProfileImage.Create(Guid.NewGuid(), command.FileName, command.FileContentType.MediaType, command.IsPrimary);

            var updatedUser =
                await this.usersService.EditProfileImageAsync(existingUser, userProfileImage, command.FileBytes);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return this.mapper.Map<UserDto>(updatedUser);
        }
    }
}