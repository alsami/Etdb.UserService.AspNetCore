using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Cqrs.EventHandler.Users
{
    public class UserAuthenticatedEventHandler : INotificationHandler<UserAuthenticatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserAuthenticatedEventHandler> logger;
        private readonly IUsersRepository usersRepository;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IMessageProducerAdapter messageProducerAdapter;

        public UserAuthenticatedEventHandler(IUsersRepository usersRepository,
            IResourceLockingAdapter resourceLockingAdapter, IMapper mapper,
            ILogger<UserAuthenticatedEventHandler> logger, IMessageProducerAdapter messageProducerAdapter)
        {
            this.usersRepository = usersRepository;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.mapper = mapper;
            this.logger = logger;
            this.messageProducerAdapter = messageProducerAdapter;
        }

        public async Task Handle(UserAuthenticatedEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                await this.messageProducerAdapter.ProduceAsync(@event, MessageType.UserAuthenticated);
                
                var user = await this.usersRepository.FindAsync(@event.UserId);

                if (!await this.resourceLockingAdapter.LockAsync(user!.Id, TimeSpan.FromSeconds(30)))
                {
                    throw WellknownExceptions.UserResourceLockException(user.Id);
                }

                var signInLog = this.mapper.Map<AuthenticationLog>(@event);
                user.AddAuthenticationLog(signInLog);

                await this.usersRepository.EditAsync(user);

                await this.resourceLockingAdapter.UnlockAsync(user.Id);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
            }
        }
    }
}