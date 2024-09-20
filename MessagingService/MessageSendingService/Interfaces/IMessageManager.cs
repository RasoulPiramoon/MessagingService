using MessageSendingService.Classes;

namespace MessageSendingService.Interface
{
    internal interface IMessageManager
    {
        Task<(Message message, bool isSuccess)> SendMessage(Message message);
    }
}
