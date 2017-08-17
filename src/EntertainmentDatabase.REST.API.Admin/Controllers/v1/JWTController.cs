using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class JWTController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<ApplicationUser> applicationUserRepo;

        public JWTController(IMapper mapper, IEntityRepository<ApplicationUser> applicationUserRepo)
        {
            this.mapper = mapper;
            this.applicationUserRepo = applicationUserRepo;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateToken([FromBody] ApplicationUserDTO applicationUserDTO)
        {
            var applicationUser = this.applicationUserRepo.Get(applicationUserDTO.Id);

            return null;
        }
    }
}
