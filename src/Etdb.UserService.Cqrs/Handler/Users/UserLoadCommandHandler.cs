using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Handler.Users
{
    public class UserLoadCommandHandler : IResponseCommandHandler<UserLoadCommand, UserDto>
    {
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public UserLoadCommandHandler(IUsersService usersService, IMapper mapper)
        {
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(UserLoadCommand request, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found!");
            }

            return this.mapper.Map<UserDto>(user);
        }
    }
}