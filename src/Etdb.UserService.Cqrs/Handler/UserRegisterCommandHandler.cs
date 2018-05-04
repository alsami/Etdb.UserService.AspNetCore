using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cqrs.Handler;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserRegisterCommandHandler : VoidCommandHandler<UserRegisterCommand>
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public UserRegisterCommandHandler(IVoidCommandValidation<UserRegisterCommand> commandValidation, 
            IMapper mapper, IUserService userService) : base(commandValidation)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        public override async Task Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var validation = await this.CommandValidation.ValidateCommandAsync(request);

            if (!validation.IsValid)
            {
                throw validation.GenerateValidationException("Error validating user registration!");
            }

            var user = this.mapper.Map<User>(request);

            await this.userService.RegisterAsync(user);
        }
    }
}