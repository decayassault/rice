using System.Collections.Generic;
using System;
namespace Data
{
    public interface IMemory//напрямую в память обращаться нельзя; разбить на интерфейсы
    {
        IEnumerable<Pair> LoginPasswordHashesDeltaKeys { get; }
        string PageToReturnRegistrationData { get; set; }
        int DialogsToStartCount { get; }
        int PersonalMessagesToPublishCount { get; }
        string LastPage { get; }
        int PagesLength { get; }
        int ThreadsCount { get; }
        string[] Pages { get; }
        string Temp { get; set; }
        int Pos { get; set; }
        int TopicsToStartCount { get; }
        int MessagesToPublishCount { get; }
        int PreRegistrationLineCount { get; }
        int CaptchaMessagesRegistrationDataCount { get; }
        int RegistrationLineCount { get; }
        int CaptchaMessagesCount { get; }
        ICollection<Pair> LoginPasswordHashesKeys { get; }
        string CaptchaPageToReturnLogin { get; set; }
        bool LoginPasswordAccIdHashesContainsKey(Pair pair);
        void SetTimerDivider(byte value);
        byte GetTimerDivider();
        void IncrementTimerDivider();
        bool TimerDividerIsDivider(byte value);
        void LoginPasswordAccIdHashesTryAdd(Pair pair, int accountId);
        void LoginPasswordHashesDeltaTryRemove(Pair pair, out byte result);
        void InitializeLoginPasswordAccIdHashes();
        void InitializeLoginPasswordHashes();
        bool CaptchaMessagesContains(uint captcha);
        void InitializeLoginPasswordHashesDelta();
        bool LoginPasswordHashesContainsKey(Pair pair);
        void LoginPasswordHashesTryAdd(Pair pair, Guid? guid);
        bool NicksHashesKeysContains(uint hash);
        void InitializeNicksHashes();
        void NicksHashesTryAdd(uint nickHash, byte temp);
        string GetEndPointPageLocked(int index);
        void SetEndPointPageLocked(int index, string value);
        void InitializeEndPointPagesLocked(int size);
        string GetMainContentLocked();
        void SetMainContentLocked(string value);
        string GetMainPageLocked();
        void SetMainPageLocked(string value);
        void AddToMainPageLocked(string value);
        void DialogsToStartEnqueue(DialogData value);
        void InitializeDialogsToStart();
        void DialogsToStartTryDequeue(out DialogData value);
        void InitializePersonalMessagesToPublish();
        void PersonalMessagesToPublishEnqueue(MessageData value);
        void PersonalMessagesToPublishTryDequeue(out MessageData value);
        string[] GetDialogPagesArrayLocked(int index);
        void SetDialogPagesArrayLocked(int index, string[] value);
        string GetDialogPagesPageLocked(int arrayIndex, int pageIndex);
        void SetDialogPagesPageLocked(int arrayIndex, int pageIndex, string value);
        void AddToDialogPagesPageLocked(int arrayIndex, int pageIndex, string value);
        void InitializeDialogPagesLocked(string[][] value);
        int GetDialogPagesLengthLocked();
        void SetDialogPagesLengthLocked(int value);
        void CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked();
        void SetDialogPagesPageDepthLocked(int index, int value);
        int GetDialogPagesPageDepthLocked(int index);
        void InitializeDialogPagesPageDepthLocked(int[] value);
        string GetMessage(int ownerId, int companionId, int messageId);
        string[] GetMessages(int ownerId, int companionId);
        void AddToPersonalPagesDepth(int id, int accountId);
        int GetPersonalPagesDepth(int id, int accountId);
        void SetPersonalPagesPage
                    (int id, int accountId, int depth, string page);
        void SetPersonalPagesMessagesArray
                        (int id, int accountId, string[] value);
        void InitializePrivateMessages();
        bool PersonalPagesContainsKey(OwnerId ownerId, CompanionId companionId, bool flag);
        void PersonalPagesAdd(OwnerId ownerId, CompanionId companionId, string[] newMsg, bool flag);
        bool PersonalPagesDepthsContainsKey(OwnerId ownerId, CompanionId companionId, bool flag);
        void PersonalPagesDepthsAdd(OwnerId ownerId, CompanionId companionId, bool flag);
        bool PersonalPagesDepthsKeysContains(OwnerId ownerId);
        bool PersonalPagesKeysContains(OwnerId ownerId);
        int GetPersonalPagesPageDepth(int accountId, int companionId);
        void PersonalPagesTryAdd(OwnerId ownerId, Dictionary<CompanionId, PrivateMessages> temp1);
        void PersonalPagesDepthsTryAdd(OwnerId ownerId,
                                                    Dictionary<CompanionId, int> temp2);
        string[] GetSectionPagesArrayLocked(int index);
        void SetSectionPagesArrayLocked(int index, string[] value);
        string GetSectionPagesPageLocked(int arrayIndex, int pageIndex);
        void SetSectionPagesPageLocked(int arrayIndex, int pageIndex, string value);
        void AddToSectionPagesPageLocked(int arrayIndex, int pageIndex, string value);
        void InitializeSectionPagesLocked(string[][] value);
        int GetSectionPagesLengthLocked();
        void SetSectionPagesLengthLocked(int value);
        void SetSectionPagesPageDepthLocked(int index, int value);
        int GetSectionPagesPageDepthLocked(int index);
        void InitializeSectionPagesPageDepthLocked(int[] value);
        void InitializeTopicsToStart();
        void SetSectionPagesArray(int endpointId);
        void SetThreadsCount(int threadsCount);
        void SetPages(string[] temp);
        void SetLastPage(string value);
        string GetPage(int num);
        void SetPage(int index, string value);
        void TopicsToStartEnqueue(TopicData value);
        void TopicsToStartTryDequeue(out TopicData topicData);
        void SetThreadPagesPageLocked(int arrayIndex, int pageIndex, string value);
        string GetThreadPagesPageLocked(int arrayIndex, int pageIndex);
        string[] GetThreadPagesArrayLocked(int arrayIndex);
        void SetThreadPagesArrayLocked(int arrayIndex, string[] value);
        void AddToThreadPagesPageLocked(int arrayIndex, int pageIndex, string value);
        void InitializeThreadPagesLocked(string[][] value);
        string[][] GetThreadPagesLocked();
        int GetThreadPagesPageDepthLocked(int index);
        void AddToThreadPagesPageDepthLocked(int index, int value);
        void SetThreadPagesPageDepthLocked(int index, int value);
        void InitializeThreadPagesPageDepthLocked(int[] value);
        int[] GetThreadPagesPageDepthLocked();
        int GetThreadPagesLengthLocked();
        void SetThreadPagesLengthLocked(int value);
        void InitializeMessagesToPublish();
        void MessagesToPublishEnqueue(MessageData messageData);
        void MessagesToPublishTryDequeue(out MessageData value);
        int GetLoginPasswordAccIdHashes(Pair pair);
        bool PreRegistrationLineTryAdd(int val, PreRegBag bag);
        void InitializePreRegistrationLine();
        void InitializeRegistrationLine();
        void CaptchaMessagesRegistrationDataEnqueue(uint toQueue);
        void CaptchaMessagesRegistrationDataDequeue();
        void InitializeCaptchaMessagesRegistrationData();
        bool CaptchaMessagesRegistrationDataContains(uint captcha);
        void RegistrationLineTryRemove(int i, out RegBag regBag);
        void PreRegistrationLineTryRemove(int key, out PreRegBag preRegBag);
        bool LoginPasswordHashesDeltaContainsKey(Pair pair);
        void LoginPasswordHashesDeltaTryAdd(Pair pair, byte val);
        void RegistrationLineTryAdd(int val, RegBag regBag);
        void CaptchaMessagesEnqueue(uint toQueue);
        void InitializeCaptchaMessages();
        void CaptchaMessagesDequeue();
        bool LoginPasswordHashesValuesContains(Guid guid);
        void SetLoginPasswordHashesPairToken(Pair pair, Guid? token);
        Guid? LoginPasswordHashesToken(Pair key);
    }
}