using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserLoadCommandHandler : IResponseCommandHandler<UserLoadCommand, UserDto>
    {
        private readonly IUsersSearchService usersSearchService;
        private readonly IMapper mapper;
        
        public UserLoadCommandHandler(IUsersSearchService usersSearchService, IMapper mapper)
        {
            this.usersSearchService = usersSearchService;
            this.mapper = mapper;
        }
        
        public async Task<UserDto> Handle(UserLoadCommand request, CancellationToken cancellationToken)
        {
            var user = await this.usersSearchService.FindUserByIdAsync(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found!");
            }

            return this.mapper.Map<UserDto>(user);
        }
    }
}