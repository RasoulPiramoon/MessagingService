using Core.Classes;
using Core.Interface;

namespace MessageSendingService.Classes
{
    internal class SmsManager : IMessageManager
    {
        public async Task<(Message message, bool isSuccess)> SendMessage(Message message)
        {
            await Task.Delay(1000);
            //Code for sending Sms
            Console.WriteLine($"Sms with id: {message.Id} sent to customer: {message.Customer}");

            return (message, true);
        }
    }
}
