using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Extensions;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler
{
    // ReSharper disable once UnusedMember.Global
    public class UserProfileImageAddCommandHandler : IResponseCommandHandler<UserProfileImageAddCommand, UserDto>
    {
        private readonly IOptions<FileStoreOptions> fileStoreOptions;
        private readonly IUsersService usersService;
        private readonly IFileService fileService;
        private readonly IMapper mapper;

        public UserProfileImageAddCommandHandler(IOptions<FileStoreOptions> fileStoreOptions,
            IUsersService usersService, IFileService fileService, IMapper mapper)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersService = usersService;
            this.fileService = fileService;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(UserProfileImageAddCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindUserByIdAsync(request.Id);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found");
            }

            if (existingUser.ProfileImage != null)
            {
                this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath,
                    existingUser.Id.ToString(),
                    existingUser.ProfileImage.Name));
            }

            var profileImageId = Guid.NewGuid();

            var userProfileImage = new UserProfileImage(profileImageId,
                $"{profileImageId}_{DateTime.UtcNow.Ticks}_{request.FileName}",
                request.FileName, request.FileContentType.MediaType);

            await this.fileService.StoreBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, existingUser.Id.ToString()), userProfileImage.Name,
                request.FileBytes);

            var emailClones = existingUser.Emails.Select(email => email.Clone()).ToArray();

            var updatedUser = new User(existingUser.Id, existingUser.UserName, existingUser.FirstName,
                existingUser.Name, existingUser.Password, existingUser.Salt,
                existingUser.RegisteredSince, userProfileImage, existingUser.RoleIds, emailClones);

            await this.usersService.EditUserAsync(updatedUser);

            return this.mapper.Map<UserDto>(updatedUser);
        }
    }
}