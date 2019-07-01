using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;
using Etdb.UserService.Services.Abstractions.Models;

namespace Etdb.UserService.Cqrs.CommandHandler.ProfileImages
{
    // ReSharper disable once UnusedMember.Global
    public class ProfileImageAddCommandHandler : IResponseCommandHandler<ProfileImageAddCommand, ProfileImageMetaInfoDto>
    {
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IMapper mapper;

        public ProfileImageAddCommandHandler(IUsersService usersService, IMapper mapper,
            IResourceLockingAdapter resourceLockingAdapter)
        {
            this.usersService = usersService;
            this.mapper = mapper;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<ProfileImageMetaInfoDto> Handle(ProfileImageAddCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found");
            }

            if (!await this.resourceLockingAdapter.LockAsync(user.Id, TimeSpan.FromSeconds(30)))
                throw WellknownExceptions.UserResourceLockException(user.Id);

            var profileImage = ProfileImage.Create(Guid.NewGuid(),
                user.Id,
                command.FileName,
                command.FileContentType.MediaType,
                !user.ProfileImages.Any());

            var profileImageMetaInfo = new StoreImageMetaInfo(profileImage, command.FileBytes);

            await this.usersService.EditAsync(user, profileImageMetaInfo);

            await this.resourceLockingAdapter.UnlockAsync(user.Id);

            return this.mapper.Map<ProfileImageMetaInfoDto>(profileImage);
        }
    }
}