using System.Collections.Generic;
namespace Data
{
    public sealed partial class Memory
    {
        public IEnumerable<Pair> LoginPasswordHashesDeltaKeys => LoginPasswordHashesDelta.Keys;
        public int DialogsToStartCount => DialogsToStart.Count;
        public int PersonalMessagesToPublishCount => PersonalMessagesToPublish.Count;
        public string LastPage => pages[pages.Length - 1];
        public int PagesLength => pages.Length;
        public int ThreadsCount => threadsCount;
        public string[] Pages => pages;
        public string Temp { get { return temp; } set { temp = value; } }
        public int Pos { get { return pos; } set { pos = value; } }
        public int TopicsToStartCount => TopicsToStart.Count;
        public int MessagesToPublishCount => MessagesToPublish.Count;
        public int PreRegistrationLineCount => PreRegistrationLine.Count;
        public int CaptchaMessagesRegistrationDataCount
            => CaptchaMessages_RegistrationData.Count;
        public string PageToReturnRegistrationData
        {
            get { return PageToReturn_RegistrationData; }
            set { PageToReturn_RegistrationData = value; }
        }
        public int RegistrationLineCount => RegistrationLine.Count;
        public int CaptchaMessagesCount => CaptchaMessages.Count;
        public string CaptchaPageToReturnLogin
        {
            get { return CaptchaPageToReturn; }
            set { CaptchaPageToReturn = value; }
        }
        public ICollection<Pair> LoginPasswordHashesKeys => LoginPasswordHashes.Keys;
    }
}