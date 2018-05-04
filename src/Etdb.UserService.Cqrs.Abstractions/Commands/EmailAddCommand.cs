using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class EmailAddCommand : IVoidCommand
    {
        public string Address { get; set; }

        public bool IsPrimary { get; set; }
    }
}