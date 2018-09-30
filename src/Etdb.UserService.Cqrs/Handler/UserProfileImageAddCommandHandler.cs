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
    public class UserProfileImageAddCommandHandler : IResponseCommandHandler<UserProfileImageAddCommand, UserDto>
    {
        private readonly IOptions<FileStoreOptions> fileStoreOptions;
        private readonly IUsersSearchService usersSearchService;
        private readonly IUserChangesService userChangesService;
        private readonly IFileService fileService;
        private readonly IMapper mapper;

        public UserProfileImageAddCommandHandler(IOptions<FileStoreOptions> fileStoreOptions,
            IUsersSearchService usersSearchService, IFileService fileService, IUserChangesService userChangesService,
            IMapper mapper)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersSearchService = usersSearchService;
            this.fileService = fileService;
            this.userChangesService = userChangesService;
            this.mapper = mapper;
        }
        
        public async Task<UserDto> Handle(UserProfileImageAddCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersSearchService.FindUserByIdAsync(request.Id);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found");
            }

            if (existingUser.ProfileImage != null)
            {
                this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath, existingUser.ProfileImage.Name));
            }

            var profileImageId = Guid.NewGuid();

            var userProfileImage = new UserProfileImage(profileImageId, $"{profileImageId}_{DateTime.UtcNow.Ticks}_{request.FileName}",
                request.FileName, request.FileContentType.MediaType);

            await this.fileService.StoreBinaryAsync(Path.Combine(this.fileStoreOptions.Value.ImagePath, existingUser.Id.ToString()), userProfileImage.Name,
                request.FileBytes);
            
            var updatedUser = new User(existingUser.Id, existingUser.UserName, existingUser.FirstName, existingUser.Name, existingUser.Password, existingUser.Salt,
                existingUser.RegisteredSince, userProfileImage, existingUser.RoleIds, existingUser.Emails.Select(email => email.Copy()).ToArray());

            await this.userChangesService.EditUserAsync(updatedUser);
            
            // TODO: check if edited and then do what?

            return this.mapper.Map<UserDto>(updatedUser);
        }
    }
}