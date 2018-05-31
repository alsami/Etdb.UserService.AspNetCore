using System.Security.Cryptography;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    public class ContactRequestsController : Controller
    {
        private readonly IMediatorHandler mediator;

        public ContactRequestsController(IMediatorHandler mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ContactRequestDto> Create([FromBody] ContactRequestCommand contactRequestCommand)
        {
            var response =
                await this.mediator.SendCommandAsync<ContactRequestCommand, ContactRequestDto>(contactRequestCommand);

            return response;
        }
    }
}