using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Misc.Exceptions;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public AuthController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ApplicationUserDTO> Register([FromBody] LoginUserDTO loginUserDTO)
        {
            var applicationUser = new ApplicationUser
            {
                Email = loginUserDTO.Email,
                UserName = loginUserDTO.UserName ?? loginUserDTO.Email
            };

            var result = await this.userManager.CreateAsync(applicationUser, loginUserDTO.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(applicationUser, false);

                return this.mapper.Map<ApplicationUserDTO>(applicationUser);
            }

            throw new RegisterException
            {
                IdentityErrors = result.Errors
            };
        }

        [HttpPost("login")]
        public async Task<ApplicationUserDTO> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            var user = await this.userManager.FindByNameAsync(loginUserDTO.UserName);

            if(user != null)
            {
                var result = await this.signInManager.PasswordSignInAsync(user, loginUserDTO.Password, false, false);

                if (result.Succeeded)
                {
                    return this.mapper.Map<ApplicationUserDTO>(user);
                }
            }

            throw new LoginFailedException("Incorrect password or username");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return Ok();
        }
    }
}
