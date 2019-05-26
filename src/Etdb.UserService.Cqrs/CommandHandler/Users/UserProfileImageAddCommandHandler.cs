using System;
using System.Linq;
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
using Etdb.UserService.Services.Abstractions.Models;

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
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found");
            }

            if (!await this.resourceLockingAdapter.LockAsync(user.Id, TimeSpan.FromSeconds(30)))
                throw WellknownExceptions.UserResourceLockException(user.Id);

            var profileImageMetaInfo = new ProfileImageMetaInfo(ProfileImage.Create(Guid.NewGuid(),
                    user.Id,
                    command.FileName,
                    command.FileContentType.MediaType,
                    !user.ProfileImages.Any()),
                command.FileBytes);

            await this.usersService.EditAsync(user, profileImageMetaInfo);

            await this.resourceLockingAdapter.UnlockAsync(user.Id);

            return this.mapper.Map<UserDto>(user);
        }
    }
}