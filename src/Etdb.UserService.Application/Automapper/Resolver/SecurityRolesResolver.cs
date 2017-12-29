using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.DataTransferObjects;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Application.Automapper.Resolver
{
    public class SecurityRolesResolver : IValueResolver<User, UserDto, ICollection<SecurityRoleDto>>
    {
        private readonly ISecurityRoleRepository securityRoleRepository;
        private readonly IMapper mapper;

        public SecurityRolesResolver(ISecurityRoleRepository securityRoleRepository, IMapper mapper)
        {
            this.securityRoleRepository = securityRoleRepository;
            this.mapper = mapper;
        }

        public ICollection<SecurityRoleDto> Resolve(User source, UserDto destination, ICollection<SecurityRoleDto> destMember, ResolutionContext context)
        {
            var destinationRoles = source.SecurityRoles.Select(roleRef =>
            {
                var role = this.securityRoleRepository.GetAsync(Guid.Parse(roleRef.Id.AsString));
                return this.mapper.Map<SecurityRoleDto>(role.Result);
            }).ToList();

            return destinationRoles;
        }
    }
}
