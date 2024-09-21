using Core.Enums;

namespace Core.Classes
{
    public class Message
    {
        public Message(string customer, string email, string mobileNo, MessageTypeEnum messageType, string content, DateTime createdOn)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            Email = email;
            MobileNo = mobileNo;
            MessageType = messageType;
            Content = content;
            CreatedOn = createdOn;
        }

        public Guid Id { get; set; }
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
