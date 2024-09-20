using MessageSendingService.Enums;

namespace MessageSendingService.Classes
{
    internal class Message
    {
        public Message(int id, string customer, string email, string mobileNo, MessageTypeEnum messageType, string content, DateTime createdOn)
        {
            Id = id;
            Customer = customer;
            Email = email;
            MobileNo = mobileNo;
            MessageType = messageType;
            Content = content;
            CreatedOn = createdOn;
        }

        public int Id { get; set; }
        public string Customer { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public MessageStatusEnum MessageStatus { get; set; }
        public DateTime? SentTime { get; set; }

        public void UpdateStatus(bool isSuccess)
        {
            if (isSuccess)
            {
                MessageStatus = MessageStatusEnum.Sent;
                SentTime = DateTime.UtcNow;
            }
            else
                MessageStatus = MessageStatusEnum.Failed;
        }
    }
}
