namespace Forum.Models.Threads
{
    using System.Threading;
    using Forum.Data;
    using Forum.Data.NewPrivateMessage;
    internal sealed class PersonalMessenger
    {
        private static Thread PersonalMessagePublisher;
        private static object PersonalMessagePublisherLock = new object();
        internal static void Start()
        {
            InitializeThread();
            SetPriority();
            StartThread();
        }
        private static void InitializeThread()
        {
            lock (PersonalMessagePublisherLock)
                PersonalMessagePublisher = 
                    new Thread(NewPrivateMessageLogic
                            .PublishNextPrivateMessage);
        }
        private static void SetPriority()
        {
            lock (PersonalMessagePublisherLock)
                PersonalMessagePublisher.Priority = ThreadPriority.Lowest;
        }
        private static void StartThread()
        {
            lock (PersonalMessagePublisherLock)
                PersonalMessagePublisher.Start();
        }
    }
}