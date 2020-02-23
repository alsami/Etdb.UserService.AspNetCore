using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.EventHandler.Users
{
    public class UserAuthenticatedEventHandler : INotificationHandler<UserAuthenticatedEvent>
    {
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepository;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IMessageProducerAdapter messageProducerAdapter;

        public UserAuthenticatedEventHandler(IUsersRepository usersRepository,
            IResourceLockingAdapter resourceLockingAdapter, IMapper mapper, IMessageProducerAdapter messageProducerAdapter)
        {
            this.usersRepository = usersRepository;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.mapper = mapper;
            this.messageProducerAdapter = messageProducerAdapter;
        }

        public async Task Handle(UserAuthenticatedEvent @event, CancellationToken cancellationToken)
        {
            await this.messageProducerAdapter.ProduceAsync(@event, MessageType.UserAuthenticated);

            var user = await this.usersRepository.FindAsync(@event.UserId);

            if (!await this.resourceLockingAdapter.LockAsync(user!.Id, TimeSpan.FromSeconds(30)))
                throw WellknownExceptions.UserResourceLockException(user.Id);

            var authenticationLog = this.mapper.Map<AuthenticationLog>(@event);

            user.AddAuthenticationLog(authenticationLog);

            await this.usersRepository.EditAsync(user);
            
            await this.resourceLockingAdapter.UnlockAsync(user.Id);
        }
    }
}