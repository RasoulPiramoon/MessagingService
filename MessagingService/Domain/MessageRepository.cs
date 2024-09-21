using Core.Classes;

namespace Core
{
    //برای نگهداری مسیج ها از این کلاس استفده کردم. می توان از دیتابیس یا مسیج بروکر ها استفاده کرد.برای سادگی از کلاس استفاده کردم
    public static class MessageRepository
    {
        private static List<Message> _messages = new();
        private static readonly ReaderWriterLockSlim _lock = new();

        public static void AddMesssage(Message message)
        {
            _lock.EnterWriteLock();
            try
            {
                _messages.Add(message);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public static IEnumerable<Message> GetAllMessages()
        {
            _lock.EnterReadLock();
            try
            {
                return _messages;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
