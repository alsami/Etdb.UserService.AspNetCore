using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.ServiceBase.Generics.Base;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<User> useRepository;
        private readonly IEntityRepository<Securityrole> securityRoleRepository;

        public UsersController(IMapper mapper, 
            IEntityRepository<User> useRepository, 
            IEntityRepository<Securityrole> securityRoleRepository)
        {
            this.mapper = mapper;
            this.useRepository = useRepository;
            this.securityRoleRepository = securityRoleRepository;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public UserDTO Registration([FromBody] RegisterUserDTO registerUserDTO)
        {
            var userExists = this.useRepository
                .Get(existingUser => existingUser.UserName.Equals(registerUserDTO.UserName, StringComparison.OrdinalIgnoreCase)
                             || existingUser.Email.Equals(registerUserDTO.Email, StringComparison.OrdinalIgnoreCase)) != null;

            if (userExists)
            {
                throw new Exception("Username or email already taken!");
            }

            var user = this.mapper.Map<User>(registerUserDTO);
            user.UserSecurityroles.Add(new UserSecurityrole
            {
                User = user,
                Securityrole = this.securityRoleRepository.Get(role => role.Designation == RoleNames.User)
            });

            this.useRepository.Add(user);
            this.useRepository.EnsureChanges();

            return this.mapper.Map<UserDTO>(user);
        }
    }
}
