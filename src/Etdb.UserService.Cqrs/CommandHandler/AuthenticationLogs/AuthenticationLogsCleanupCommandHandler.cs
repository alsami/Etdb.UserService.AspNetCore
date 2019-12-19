using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Cqrs.CommandHandler.AuthenticationLogs
{
    public class AuthenticationLogsCleanupCommandHandler : IRequestHandler<AuthenticationLogsCleanupCommand>
    {
        private readonly IUsersRepository usersRepository;
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly ILogger<AuthenticationLogsCleanupCommandHandler> logger;

        public AuthenticationLogsCleanupCommandHandler(IUsersRepository usersRepository,
            IResourceLockingAdapter resourceLockingAdapter, ILogger<AuthenticationLogsCleanupCommandHandler> logger,
            IUsersService usersService)
        {
            this.usersRepository = usersRepository;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.logger = logger;
            this.usersService = usersService;
        }

        public async Task<Unit> Handle(AuthenticationLogsCleanupCommand command, CancellationToken cancellationToken)
        {
            var expiredAt = DateTime.UtcNow - command.LogsOlderThanSpan;

            var users = await this.usersRepository.FindAllAsync(user =>
                user.AuthenticationLogs.Any(log => log.LoggedAt <= expiredAt));

            var enumeratedUsers = users as User[] ?? users.ToArray();

            if (!enumeratedUsers.Any()) return Unit.Value;

            var semaphoreSlim = new SemaphoreSlim(5);

            var cleanupTasks = enumeratedUsers.Select(async user =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync(cancellationToken);

                    if (!await this.resourceLockingAdapter.LockAsync(user.Id, TimeSpan.FromMinutes(1)))
                    {
                        this.logger.LogInformation("Couldn't clear logs for user {id}, user resource currently busy!",
                            user.Id);

                        return Task.CompletedTask;
                    }

                    var previousValue = user.AuthenticationLogs.Count;

                    user.RemoveAuthenticationLogs(log => log.LoggedAt <= expiredAt);

                    await this.usersService.EditAsync(user);

                    var currentValue = user.AuthenticationLogs.Count;

                    this.logger.LogInformation("Cleanup {amount} of authentication logs for user {id}",
                        previousValue - currentValue, user.Id);

                    await this.resourceLockingAdapter.UnlockAsync(user.Id);

                    return Task.CompletedTask;
                }
                finally
                {
                    semaphoreSlim.Release(1);
                }
            });

            await Task.WhenAll(cleanupTasks);

            return Unit.Value;
        }
    }
}