using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Handler
{
    public class ContactRequestCommandHandler : IResponseCommandHandler<ContactRequestCommand, ContactRequestDto>
    {
        private readonly IMapper mapper;
        private readonly IUsersSearchService usersSearchService;
        private readonly IContactRequestRepository contactRequestRepository;

        public ContactRequestCommandHandler(IMapper mapper, IUsersSearchService usersSearchService, IContactRequestRepository contactRequestRepository)
        {
            this.mapper = mapper;
            this.usersSearchService = usersSearchService;
            this.contactRequestRepository = contactRequestRepository;
        }
        
        public async Task<ContactRequestDto> Handle(ContactRequestCommand request, CancellationToken cancellationToken)
        {
            var sender = await this.usersSearchService.FindUserByIdAsync(request.Sender);

            if (sender == null)
            {
                throw new ResourceNotFoundException("Cannot find contact request sender!");
            }

            var receiver = await this.usersSearchService.FindUserByIdAsync(request.Receiver);

            if (receiver == null)
            {
                throw new ResourceNotFoundException("Cannot find contact request receiver!");
            }

            if (sender.Contacts.Contains(receiver.Id) || receiver.Contacts.Contains(sender.Id))
            {
                // TODO
                throw new Exception("TODO");
            }
            
            throw new Exception();
        }
    }
}