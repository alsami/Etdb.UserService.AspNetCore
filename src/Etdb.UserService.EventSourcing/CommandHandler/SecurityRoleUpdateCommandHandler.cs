using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Presentation.DataTransferObjects;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.CommandHandler
{
    public class SecurityRoleUpdateCommandHandler : ITransactionHandler<SecurityRoleUpdateCommand, SecurityRoleDto>
    {
        private readonly IMapper mapper;
        private readonly ISecurityRoleRepository securityRoleRepository;

        public SecurityRoleUpdateCommandHandler(IMapper mapper, ISecurityRoleRepository securityRoleRepository)
        {
            this.mapper = mapper;
            this.securityRoleRepository = securityRoleRepository;
        }

        public Task<SecurityRoleDto> Handle(SecurityRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
