using System;
using AutoMapper;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.ServiceBase.Constants;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Presentation.DataTransferObjects;
using ETDB.API.UserService.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepo;

        public UsersController(IMapper mapper, IUserRepository userRepo)
        {
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] RegisterUserDTO registerUserDTO)
        {
            var existingUser = this.userRepo.Get(registerUserDTO.UserName, registerUserDTO.Email);

            if (existingUser != null)
            {
                throw new Exception("Username or email already taken!");
            }

            var newUser = this.mapper.Map<User>(registerUserDTO);

            this.userRepo.Register(newUser);

            return NoContent();
        }
    }
}
