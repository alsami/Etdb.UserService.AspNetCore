using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UsersSearchCommandHandler : IResponseCommandHandler<UsersSearchCommand, IEnumerable<UserFlatDto>>
    {
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepository;

        public UsersSearchCommandHandler(IUsersRepository usersRepository, IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserFlatDto>> Handle(UsersSearchCommand command, CancellationToken cancellationToken)
        {
            var loweredSearchTerm = command.SearchTerm.ToLowerInvariant();

            var users = await this.usersRepository
                .FindAllAsync(user => user.UserName.ToLowerInvariant().Contains(loweredSearchTerm)
                                      || user.Emails.Any(email => email.Address.ToLowerInvariant().Contains(loweredSearchTerm)));

            return this.mapper.Map<IEnumerable<UserFlatDto>>(users ?? Array.Empty<User>());
        }
    }
}