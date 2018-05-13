using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Extensions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserProfileImageBytesLoadCommandHandler : IResponseCommandHandler<UserProfileImageBytesLoadCommand, byte[]>
    {
        private readonly IOptions<FileStoreOptions> fileStoreOptions;
        private readonly IUsersSearchService usersSearchService;
        private readonly IFileService fileService;

        public UserProfileImageBytesLoadCommandHandler(IOptions<FileStoreOptions> fileStoreOptions, IUsersSearchService usersSearchService, IFileService fileService)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersSearchService = usersSearchService;
            this.fileService = fileService;
        }
        
        public async Task<byte[]> Handle(UserProfileImageBytesLoadCommand request, CancellationToken cancellationToken)
        {
            var user = await this.usersSearchService.FindUserByIdAsync(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found!");
            }

            if (user.ProfileImage == null)
            {
                throw new ResourceNotFoundException("The requested user does not have a profile image!");
            }

            return await this.fileService.ReadBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, user.Id.ToString()), user.ProfileImage.Name);
        }
    }
}