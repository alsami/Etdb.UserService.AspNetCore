using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using ETDB.API.UserService.Application.Services;
using ETDB.API.UserService.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IUserAppService userAppService;

        public UsersController(IDomainNotificationHandler<DomainNotification> notifications, 
            IUserAppService userAppService) : base(notifications)
        {
            this.userAppService = userAppService;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] RegisterUserDTO registerUserDTO)
        {
            this.userAppService.Register(registerUserDTO);
            return Response(NoContent());
        }
    }
}
