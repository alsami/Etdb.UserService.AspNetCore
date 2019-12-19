using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class
        UserNameAvailibilityCheckCommandHandler : IRequestHandler<UserNameAvailabilityCheckCommand,
            UserNameAvailabilityDto>
    {
        private readonly IUsersService usersService;

        public UserNameAvailibilityCheckCommandHandler(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<UserNameAvailabilityDto> Handle(UserNameAvailabilityCheckCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByUserNameAsync(request.UserName);

            return new UserNameAvailabilityDto(existingUser == null);
        }
    }
}