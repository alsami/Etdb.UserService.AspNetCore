using Etdb.ServiceBase.EventSourcing.Abstractions.Commands;

namespace Etdb.UserService.EventSourcing.Abstractions.Commands
{
    public abstract class UserCommand<TResponse> : TransactionCommand<TResponse> where TResponse : class
    {
        public string Id { get; set; }

        public byte[] RowVersion { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public byte[] Salt { get; set; }

        public string Password { get; set; }
    }
}
