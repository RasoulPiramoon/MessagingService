// See https://aka.ms/new-console-template for more information
using MessageSendingService.Classes;
using MessageSendingService.Enums;
using MessageSendingService.Interface;

Console.WriteLine("Start sending messages");
List<Message> messages = new List<Message>();
//تعداد اس ام اس یا ایمیلی که می توان به صورت همزمان ارسال کرد
Int16 concurrentMesssgeCount = 5;
List<Message>? smsMessages = null;
List<Message>? emailMessages = null;
IMessageManager smsManager = new SmsManager();
IMessageManager emailManager = new EmailManager();
//برای نگهداری تسک های همزمان
var smsTasks = new List<Task<(Message message, bool isSuccess)>>();
var emailTasks = new List<Task<(Message message, bool isSuccess)>>();

// برای اینکه ممکن است زمان لازم برای ارسال ایمیل و اس ام اس متفاوت باشد متد ارسال ایمیل و اس ام اس را جدا گرفتم
var task1 = ManageSms();
var task2 = ManageEmail();
await Task.WhenAll(task1);

async Task ManageSms()
{
    while (true)
    {
        smsMessages = GetMessages().Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.SMS).Take(concurrentMesssgeCount).ToList();
        if (smsMessages is not null && smsMessages.Any())
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
            smsTasks.RemoveAll(c => 1 == 1);
        }
        else
        {
            Console.WriteLine("There is no Sms for sending, sleep for 5 second");
            await Task.Delay(5000);
        }
    }
}

async Task ManageEmail()
{
    while (true)
    {
        emailMessages = GetMessages().Where(c => c.MessageStatus == MessageStatusEnum.New && c.MessageType == MessageTypeEnum.Email).Take(concurrentMesssgeCount).ToList();
        if (emailMessages is not null && emailMessages.Any())
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
            emailTasks.RemoveAll(c => 1 == 1);
        }
        else
        {
            Console.WriteLine("There is no email for sending, sleep for 5 second");
            await Task.Delay(5000);
        }
    }
}

List<Message> GetMessages()
{
    // تعدادی مسیج برای تست ایجاد می کنیم
    if (!messages.Any())
    {
        messages = [
            new Message(1, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 1)),
        new Message(2, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.Email, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 10)),
        new Message(3, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 50, 20)),
        new Message(4, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 50, 30)),
        new Message(5, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 40)),
        new Message(6, "Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 42)),
        new Message(7, "Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.Email, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 44)),
        new Message(8, "Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 50)),
        new Message(9, "Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.Email, "Hello Customer3", new DateTime(2024, 9, 20, 22, 50, 52)),
        new Message(10, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 55)),
        new Message(11, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 50, 59)),
        new Message(12, "Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.SMS, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 1)),
        new Message(13, "Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.SMS, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 5)),
        new Message(14, "Customer4", "Customer4@Yahoo.com", "09374840785", MessageTypeEnum.Email, "Hello Customer4", new DateTime(2024, 9, 20, 22, 51, 10)),
        new Message(15, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 15)),
        new Message(16, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 20)),
        new Message(17, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.SMS, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 25)),
        new Message(18, "Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.Email, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 33)),
        new Message(19, "Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.SMS, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 38)),
        new Message(20, "Customer5", "Customer5@Yahoo.com", "09374840810", MessageTypeEnum.SMS, "Hello Customer5", new DateTime(2024, 9, 20, 22, 51, 44)),
        new Message(21, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.Email, "Hello Customer1", new DateTime(2024, 9, 20, 22, 51, 48)),
        new Message(22, "Customer3", "Customer3@Yahoo.com", "09374840782", MessageTypeEnum.SMS, "Hello Customer3", new DateTime(2024, 9, 20, 22, 51, 50)),
        new Message(23, "Customer2", "Customer2@Yahoo.com", "09374840780", MessageTypeEnum.Email, "Hello Customer2", new DateTime(2024, 9, 20, 22, 51, 55)),
        new Message(24, "Customer1", "Customer1@Yahoo.com", "09374840770", MessageTypeEnum.SMS, "Hello Customer1", new DateTime(2024, 9, 20, 22, 51, 58)),
    ];
    }

    return messages;
}
