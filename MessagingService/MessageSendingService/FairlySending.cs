using Core.Classes;
using Core.Enums;
using Core.Interface;
using Core;
using MessageSendingService.Classes;

namespace MessageSendingService
{
    internal class FairlySending
    {
        //تعداد اس ام اس یا ایمیلی که می توان به صورت همزمان ارسال کرد
        Int16 concurrentMesssgeCount = 5;
        
        public FairlySending()
        {
            CreateMessages();
        }

        public async Task SendAsync()
        {
            // برای اینکه ممکن است زمان لازم برای ارسال ایمیل و اس ام اس متفاوت باشد متد ارسال ایمیل و اس ام اس را جدا گرفتم
            var task1 = ManageSms();
            var task2 = ManageEmail();
            await Task.WhenAll(task1, task2);
        }

        private async Task ManageSms()
        {
            //برای نگهداری تسک های همزمان
            List<Task<(Message message, bool isSuccess)>> smsTasks = new List<Task<(Message message, bool isSuccess)>>();
            List<Message>? smsMessages = null;
            IMessageManager smsManager = new SmsManager();
            while (true)
            {
                var customerWithMessage = MessageRepository.GetAllMessages()
                    .Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.SMS)
                    .Select(c => c.Customer)
                    .Distinct()
                    .ToList();
                if (customerWithMessage != null && customerWithMessage.Any())
                {
                    foreach (var customer in customerWithMessage)
                    {
                        smsMessages = MessageRepository.GetAllMessages()
                            .Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.SMS
                                 && c.Customer == customer)
                            .Take(concurrentMesssgeCount)
                            .ToList();
                        if (smsMessages is not null && smsMessages.Any())
                        {
                            try
                            {
                                for (int i = 0; i < smsMessages.Count; i++)
                                {
                                    smsTasks.Add(smsManager.SendMessage(smsMessages[i]));
                                }
                                var results = await Task.WhenAll(smsTasks);
                                foreach (var result in results)
                                {
                                    result.message.UpdateStatus(result.isSuccess);
                                }
                                smsTasks.Clear();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("There is no Sms for sending, sleep for 5 second");
                    await Task.Delay(5000);
                }
            }
        }

        private async Task ManageEmail()
        {
            //برای نگهداری تسک های همزمان
            List<Task<(Message message, bool isSuccess)>> emailTasks = new List<Task<(Message message, bool isSuccess)>>();
            List<Message>? emailMessages = null;
            IMessageManager emailManager = new EmailManager();
            while (true)
            {
                var customerWithMessage = MessageRepository.GetAllMessages()
                    .Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.Email)
                    .Select(c => c.Customer)
                    .Distinct()
                    .ToList();

                if (customerWithMessage != null && customerWithMessage.Any())
                {
                    foreach (var customer in customerWithMessage)
                    {
                        emailMessages = MessageRepository.GetAllMessages()
                        .Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.Email
                            && c.Customer == customer)
                        .Take(concurrentMesssgeCount)
                        .ToList();
                        if (emailMessages is not null && emailMessages.Any())
                        {
                            try
                            {
                                for (int i = 0; i < emailMessages.Count; i++)
                                {
                                    emailTasks.Add(emailManager.SendMessage(emailMessages[i]));
                                }
                                var results = await Task.WhenAll(emailTasks);

                                foreach (var result in results)
                                {
                                    result.message.UpdateStatus(result.isSuccess);
                                }
                                emailTasks.Clear();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("There is no email for sending, sleep for 5 second");
                    await Task.Delay(5000);
                }
            }
        }

        private void CreateMessages()
        {
            // تعدادی مسیج برای تست ایجاد می کنیم
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 10, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 20, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 30, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 35, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 40, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 42, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 44, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 21, 48, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 1)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.Email, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 10)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 50, 20)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 50, 30)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 40)));
            MessageRepository.AddMesssage(new Message("Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 42)));
            MessageRepository.AddMesssage(new Message("Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.Email, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 44)));
            MessageRepository.AddMesssage(new Message("Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 50)));
            MessageRepository.AddMesssage(new Message("Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.Email, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 52)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 55)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 59)));
            MessageRepository.AddMesssage(new Message("Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.SMS, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 1)));
            MessageRepository.AddMesssage(new Message("Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.SMS, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 5)));
            MessageRepository.AddMesssage(new Message("Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.Email, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 10)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 15)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 20)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 25)));
            MessageRepository.AddMesssage(new Message("Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.Email, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 33)));
            MessageRepository.AddMesssage(new Message("Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.SMS, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 38)));
            MessageRepository.AddMesssage(new Message("Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.SMS, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 44)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.Email, "Hello Customer1", new DateTime(2024, 9, 20, 22, 51, 48)));
            MessageRepository.AddMesssage(new Message("Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 51, 50)));
            MessageRepository.AddMesssage(new Message("Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 55)));
            MessageRepository.AddMesssage(new Message("Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 51, 58)));
        }

    }
}
