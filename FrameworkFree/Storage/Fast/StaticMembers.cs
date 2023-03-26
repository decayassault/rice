using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
namespace Data
{
    public sealed partial class Memory
    {
        public static
            ConcurrentDictionary<Pair, int> LoginPasswordAccIdHashes;
        public static
            ConcurrentDictionary<uint, byte> NicksHashes;
        public static
            ConcurrentDictionary<Pair, Guid?> LoginPasswordHashes;
        public static
            ConcurrentDictionary<Pair, byte> LoginPasswordHashesDelta;
        public static
            ConcurrentDictionary<OwnerId, Dictionary<CompanionId, PrivateMessages>> PersonalPages;
        private static
            ConcurrentDictionary<OwnerId, Dictionary<CompanionId, int>> PersonalPagesDepths;

        internal static ConcurrentDictionary<int, RegBag> RegistrationLine;
        internal static ConcurrentDictionary<int, PreRegBag> PreRegistrationLine;
        public static ConcurrentQueue<DialogData> DialogsToStart;
        public static ConcurrentQueue<MessageData> PersonalMessagesToPublish;
        public static ConcurrentQueue<MessageData> MessagesToPublish;
        public static ConcurrentQueue<TopicData> TopicsToStart;
        public volatile static Queue<uint> CaptchaMessages;
        public volatile static Queue<uint> CaptchaMessages_RegistrationData;
        public static string[][] DialogPages;
        public static string[][] SectionPages;
        public static string[][] ThreadPages;
        public static string[] EndPointPages;
        public static string[] pages;
        public static int[] DialogPagesPageDepth;
        public static int[] SectionPagesPageDepth;
        public static int[] ThreadPagesPageDepth;
        public volatile static string CaptchaPageToReturn;
        public volatile static string PageToReturn_RegistrationData;
        public volatile static byte TimerDivider;
        private static readonly object locker = new object();
        public static string MainPage;
        public static string MainContent;
        public static string temp;
        public static int threadsCount;
        public static int pos;
        public static int DialogPagesLength;
        public static int SectionPagesLength;
        public static int ThreadPagesLength;
    }
}