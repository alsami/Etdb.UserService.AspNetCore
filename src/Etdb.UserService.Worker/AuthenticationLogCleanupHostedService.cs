using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Etdb.UserService.Cqrs.Abstractions.Commands.AuthenticationLogs;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Worker
{
    public class AuthenticationLogCleanupHostedService : BackgroundService
    {
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger<AuthenticationLogCleanupHostedService> logger;

        public AuthenticationLogCleanupHostedService(ILifetimeScope lifetimeScope,
            ILogger<AuthenticationLogCleanupHostedService> logger)
        {
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Worker {worker} started", this.GetType().GetTypeInfo().Name);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = this.lifetimeScope.BeginLifetimeScope())
                    {
                        var bus = scope.Resolve<IMediator>();

                        await bus
                            .Send(new AuthenticationLogsCleanupCommand(TimeSpan.FromDays(5)),
                                stoppingToken);
                    }

                    this.logger.LogInformation("Next authentication log cleanup at {at} UTC",
                        DateTime.UtcNow.AddHours(12)
                            .ToString($"dd.MM.yyyy hh:mm:ss", DateTimeFormatInfo.InvariantInfo));
                    await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
                }
                catch (Exception exception) when (!(exception is TaskCanceledException))
                {
                    this.logger.LogError(
                        "An error occured while sending command to clear older authentication-logs! Message:\n{message}",
                        exception.Message);
                }
            }

            this.logger.LogInformation("Worker {worker} stopping", this.GetType().GetTypeInfo().Name);
        }
    }
}