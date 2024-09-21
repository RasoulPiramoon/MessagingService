using Core.Classes;

namespace Core.Interface
{
    public interface IMessageManager
    {
        Task<(Message message, bool isSuccess)> SendMessage(Message message);
    }
}
