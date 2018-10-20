using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Models;
using Etdb.UserService.Extensions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler
{
    public class
        UserProfileImageLoadCommandHandler : IResponseCommandHandler<UserProfileImageLoadCommand, FileDownloadInfo>
    {
        private readonly IUsersService _usersService;
        private readonly IFileService fileService;
        private readonly IOptions<FileStoreOptions> fileStoreOptions;

        public UserProfileImageLoadCommandHandler(IOptions<FileStoreOptions> fileStoreOptions,
            IUsersService usersService, IFileService fileService)
        {
            this.fileStoreOptions = fileStoreOptions;
            this._usersService = usersService;
            this.fileService = fileService;
        }

        public async Task<FileDownloadInfo> Handle(UserProfileImageLoadCommand request,
            CancellationToken cancellationToken)
        {
            var user = await this._usersService.FindByIdAsync(request.Id);

            if (user == null) throw new ResourceNotFoundException("The requested user could not be found!");

            if (user.ProfileImage == null)
                throw new ResourceNotFoundException("The requested user does not have a profile image!");

            var binary = await this.fileService.ReadBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, user.Id.ToString()), user.ProfileImage.Name);

            return new FileDownloadInfo(user.ProfileImage.MediaType, user.ProfileImage.Name, binary);
        }
    }
}