using System.Threading.Tasks;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IMessageProducerAdapter
    {
        Task ProduceAsync<TMessage>(TMessage message, MessageType messageType) where TMessage : class;
    }
}