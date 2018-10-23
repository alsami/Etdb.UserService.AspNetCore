using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserProfileInfoChangeCommandHandler : IVoidCommandHandler<UserProfileInfoChangeCommand>
    {
        private readonly IUsersService usersService;

        public UserProfileInfoChangeCommandHandler(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<Unit> Handle(UserProfileInfoChangeCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.Id);

            if (existingUser == null)
            {
                throw WellknownExceptions.UserNotFoundException();
            }

            var user = new User(existingUser.Id, existingUser.UserName, command.FirstName, command.Name,
                command.Biography,
                existingUser.Password, existingUser.Salt, existingUser.RegisteredSince, existingUser.ProfileImage,
                existingUser.RoleIds, existingUser.Emails);

            await this.usersService.EditAsync(user);

            return Unit.Value;
        }
    }
}