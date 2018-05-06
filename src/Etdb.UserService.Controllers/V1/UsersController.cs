using System;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    public class UsersController : Controller
    {
        private readonly IMediatorHandler mediator;

        public UsersController(IMediatorHandler mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:Guid}")]
        public async Task<UserDto> GetUser(Guid id)
        {
            var command = new UserLoadCommand(id);

            var user = await this.mediator.SendCommandAsync<UserLoadCommand, UserDto>(command);

            return user;
        }
    }
}