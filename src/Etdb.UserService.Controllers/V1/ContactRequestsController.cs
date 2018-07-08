using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    public class ContactRequestsController : Controller
    {
        private readonly IBus bus;

        public ContactRequestsController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        public async Task<ContactRequestDto> Create([FromBody] ContactRequestCommand contactRequestCommand)
        {
            var response =
                await this.bus.SendCommandAsync<ContactRequestCommand, ContactRequestDto>(contactRequestCommand);

            return response;
        }
    }
}