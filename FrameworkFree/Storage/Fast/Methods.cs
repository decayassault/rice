using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading;
namespace Data
{ // Перекрёстные ссылки между методами исключены.
    public sealed partial class Memory
    { //TODO разбить на локеры
        public bool LoginPasswordAccIdHashesContainsKey(Pair pair)
        => LoginPasswordAccIdHashes.ContainsKey(pair);
        public byte GetTimerDivider()
        {
            lock (locker) return TimerDivider;
        }
        public void SetTimerDivider(byte value)
        {
            lock (locker) TimerDivider = value;
        }
        public void IncrementTimerDivider() // каждые дцать миллисекунд
        {
            lock (locker)
                if (TimerDivider == byte.MaxValue)
                    TimerDivider = byte.MinValue;
                else
                    TimerDivider++;
        }
        public bool TimerDividerIsDivider(byte value)
        {
            lock (locker) return TimerDivider % value > 0;
        }
        public void LoginPasswordAccIdHashesTryAdd(Pair pair, int accountId)
        => LoginPasswordAccIdHashes.TryAdd(pair, accountId);
        public void LoginPasswordHashesDeltaTryRemove(Pair pair, out byte result)
        => LoginPasswordHashesDelta.TryRemove(pair, out result);
        public void LoginPasswordHashesDeltaTryAdd(Pair pair, byte val)
        => LoginPasswordHashesDelta.TryAdd(pair, 0);
        public bool LoginPasswordHashesDeltaContainsKey(Pair pair)
        => LoginPasswordHashesDelta.ContainsKey(pair);
        public int GetLoginPasswordAccIdHashes(Pair pair)
        => LoginPasswordAccIdHashes[pair];
        public void InitializeLoginPasswordAccIdHashes()
        {
            Memory.LoginPasswordAccIdHashes = new ConcurrentDictionary<Pair, int>();
        }
        public void InitializeLoginPasswordHashes()
        {
            Memory.LoginPasswordHashes = new ConcurrentDictionary<Pair, Guid?>();
        }
        public void InitializeLoginPasswordHashesDelta()
        {
            Memory.LoginPasswordHashesDelta = new ConcurrentDictionary<Pair, byte>();
        }
        public void InitializePrivateMessages()
        {
            Memory.PersonalPages = new
                ConcurrentDictionary
                    <OwnerId, Dictionary<CompanionId, PrivateMessages>>();
            Memory.PersonalPagesDepths = new
            ConcurrentDictionary<OwnerId, Dictionary<CompanionId, int>>();
        }
        public bool LoginPasswordHashesContainsKey(Pair pair)
        => LoginPasswordHashes.ContainsKey(pair);
        public void LoginPasswordHashesTryAdd(Pair pair, Guid? guid)
        => LoginPasswordHashes.TryAdd(pair, null);
        public bool NicksHashesKeysContains(uint hash)
        => NicksHashes.Keys.Contains(hash);
        public void InitializeNicksHashes()
        {
            NicksHashes = new ConcurrentDictionary<uint, byte>();
        }
        public void NicksHashesTryAdd(uint nickHash, byte temp)
        => NicksHashes.TryAdd(nickHash, temp);
        public string GetEndPointPageLocked(int index)
        {
            lock (locker) return EndPointPages[index];
        }
        public void SetEndPointPageLocked(int index, string value)
        {
            lock (locker) EndPointPages[index] = value;
        }
        public void InitializeEndPointPagesLocked(int size)
        {
            lock (locker) EndPointPages = new string[size];
        }
        public string GetMainContentLocked()
        {
            lock (locker) return MainContent;
        }
        public void SetMainContentLocked(string value)
        {
            lock (locker) MainContent = value;
        }
        public string GetMainPageLocked()
        {
            lock (locker) return MainPage;
        }
        public void SetMainPageLocked(string value)
        {
            lock (locker) MainPage = value;
        }
        public void AddToMainPageLocked(string value)
        {
            lock (locker) MainPage += value;
        }
        public void DialogsToStartEnqueue(DialogData value)
        {
            DialogsToStart.Enqueue(value);
        }
        public void InitializeDialogsToStart()
        {
            DialogsToStart = new ConcurrentQueue<DialogData>();
        }
        public void DialogsToStartTryDequeue(out DialogData value)
        {
            DialogsToStart.TryDequeue(out value);
        }
        public void InitializePersonalMessagesToPublish()
        {
            PersonalMessagesToPublish = new ConcurrentQueue<MessageData>();
        }
        public void PersonalMessagesToPublishEnqueue(MessageData value)
        {
            PersonalMessagesToPublish.Enqueue(value);
        }
        public void PersonalMessagesToPublishTryDequeue(out MessageData value)
        {
            PersonalMessagesToPublish.TryDequeue(out value);
        }
        public string[] GetDialogPagesArrayLocked(int index)
        {
            lock (locker) return DialogPages[index];
        }
        public void SetDialogPagesArrayLocked(int index, string[] value)
        {
            lock (locker) DialogPages[index] = value;
        }
        public string GetDialogPagesPageLocked(int arrayIndex, int pageIndex)
        {
            lock (locker) return DialogPages[arrayIndex][pageIndex];
        }
        public void SetDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (locker) DialogPages[arrayIndex][pageIndex] = value;
        }
        public void AddToDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (locker) DialogPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeDialogPagesLocked(string[][] value)
        {
            lock (locker) DialogPages = value;
        }
        public int GetDialogPagesLengthLocked()
        {
            lock (locker) return DialogPagesLength;
        }
        public void CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked()
        {
            lock (locker)
            {
                int len = DialogPagesLength;
                DialogPagesLength++;
                string[][] temp = new string[DialogPagesLength][];
                Array.Copy(DialogPages, temp, len);
                DialogPages = temp;
                DialogPages[len] = new string[1] { Constants.NewDialog };
                int[] temp1 = new int[DialogPagesLength];
                Array.Copy(DialogPagesPageDepth, temp1, len);
                DialogPagesPageDepth = temp1;
                DialogPagesPageDepth[len] = 1;
            }
        }
        public void SetDialogPagesLengthLocked(int value)
        {
            lock (locker) DialogPagesLength = value;
        }
        public void SetDialogPagesPageDepthLocked(int index, int value)
        {
            lock (locker) DialogPagesPageDepth[index] = value;
        }
        public int GetDialogPagesPageDepthLocked(int index)
        {
            lock (locker) return DialogPagesPageDepth[index];
        }
        public void InitializeDialogPagesPageDepthLocked(int[] value)
        {
            lock (locker) DialogPagesPageDepth = value;
        }
        public string GetMessage(int ownerId, int companionId, int messageId)
        {
            lock (locker)
                return PersonalPages
                              [new OwnerId { Id = ownerId }]
                             [new CompanionId { Id = companionId }]
                             .Messages[messageId];
        }
        public string[] GetMessages(int ownerId, int companionId)
        {
            lock (locker)
                return PersonalPages
                    [new OwnerId { Id = ownerId }]
                    [new CompanionId { Id = companionId }]
                    .Messages;
        }
        public void AddToPersonalPagesDepth(int id, int accountId)
        {
            lock (locker) PersonalPagesDepths
                 [new OwnerId { Id = accountId }]
                 [new CompanionId { Id = id }]
                 ++;
        }
        public int GetPersonalPagesDepth(int id, int accountId)
        {
            lock (locker)
                return PersonalPagesDepths
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = id }];
        }
        public void SetPersonalPagesPage
                (int id, int accountId, int depth, string page)
        {
            lock (locker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            .Messages[depth] = page;
        }
        public void SetPersonalPagesMessagesArray
                    (int id, int accountId, string[] value)
        {
            lock (locker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            = new PrivateMessages { Messages = value };
        }
        public bool PersonalPagesContainsKey(OwnerId ownerId, CompanionId companionId, bool flag)
        => flag && PersonalPages[ownerId].ContainsKey(companionId);
        public void PersonalPagesAdd(OwnerId ownerId, CompanionId companionId, string[] newMsg, bool flag)
        {
            if (!flag)
                PersonalPages.TryAdd(ownerId, new Dictionary<CompanionId, PrivateMessages>());
            PersonalPages[ownerId].Add(companionId,
                    new PrivateMessages { Messages = newMsg });
        }
        public bool PersonalPagesKeysContains(OwnerId ownerId)
        => PersonalPages.ContainsKey(ownerId);
        public bool PersonalPagesDepthsContainsKey(OwnerId ownerId, CompanionId companionId, bool flag)
        => flag && PersonalPagesDepths[ownerId].ContainsKey(companionId);
        public void PersonalPagesDepthsAdd(OwnerId ownerId, CompanionId companionId, bool flag)
        {
            if (!flag)
                PersonalPagesDepths.TryAdd(ownerId, new Dictionary<CompanionId, int>());
            PersonalPagesDepths[ownerId].Add(companionId, 1);
        }
        public bool PersonalPagesDepthsKeysContains(OwnerId ownerId)
        => PersonalPagesDepths.ContainsKey(ownerId);
        public int GetPersonalPagesPageDepth(int accountId, int companionId)
        => PersonalPagesDepths
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = companionId }];//проверить границы 
        public void PersonalPagesTryAdd(OwnerId ownerId,
                                         Dictionary<CompanionId, PrivateMessages> temp1)
        => PersonalPages.TryAdd(ownerId, temp1);
        public void PersonalPagesDepthsTryAdd(OwnerId ownerId,
                                                Dictionary<CompanionId, int> temp2)
        => PersonalPagesDepths.TryAdd(ownerId, temp2);
        public string[] GetSectionPagesArrayLocked(int index)
        {
            lock (locker) return SectionPages[index];
        }
        public void SetSectionPagesArrayLocked(int index, string[] value)
        {
            lock (locker) SectionPages[index] = value;
        }
        public string GetSectionPagesPageLocked(int arrayIndex, int pageIndex)
        {
            lock (locker) return SectionPages[arrayIndex][pageIndex];
        }
        public void SetSectionPagesPageLocked(int arrayIndex, int pageIndex, string value)
        {
            lock (locker) SectionPages[arrayIndex][pageIndex] = value;
        }
        public void AddToSectionPagesPageLocked(int arrayIndex, int pageIndex, string value)
        {
            lock (locker) SectionPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeSectionPagesLocked(string[][] value)
        {
            lock (locker) SectionPages = value;
        }
        public int GetSectionPagesLengthLocked()
        {
            lock (locker) return SectionPagesLength;
        }
        public void SetSectionPagesLengthLocked(int value)
        {
            lock (locker) SectionPagesLength = value;
        }
        public void SetSectionPagesPageDepthLocked(int index, int value)
        {
            lock (locker) SectionPagesPageDepth[index] = value;
        }
        public int GetSectionPagesPageDepthLocked(int index)
        {
            lock (locker) return SectionPagesPageDepth[index];
        }
        public void InitializeSectionPagesPageDepthLocked(int[] value)
        {
            lock (locker) SectionPagesPageDepth = value;
        }
        public void InitializeTopicsToStart()
        {
            TopicsToStart = new ConcurrentQueue<TopicData>();
        }
        public void SetSectionPagesArray(int endpointId)
        {
            lock (locker) pages = SectionPages[endpointId - 1];
        }
        public void SetThreadsCount(int threadsCount)
        {
            Memory.threadsCount = threadsCount;
        }
        public void SetPages(string[] temp)
        {
            pages = temp;
        }
        public void SetLastPage(string value)
        {
            pages[pages.Length - 1] = value;
        }
        public string GetPage(int num)
        => pages[num];
        public void SetPage(int index, string value)
        {
            pages[index] = value;
        }
        public void TopicsToStartEnqueue(TopicData value)
        => TopicsToStart.Enqueue(value);
        public void TopicsToStartTryDequeue(out TopicData topicData)
        {
            TopicsToStart.TryDequeue(out TopicData data);
            topicData = data;
        }
        public void SetThreadPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (locker) ThreadPages[arrayIndex][pageIndex] = value;
        }
        public string GetThreadPagesPageLocked
            (int arrayIndex, int pageIndex)
        {
            lock (locker) return ThreadPages[arrayIndex][pageIndex];
        }
        public string[] GetThreadPagesArrayLocked(int arrayIndex)
        {
            lock (locker) return ThreadPages[arrayIndex];
        }
        public void SetThreadPagesArrayLocked
                (int arrayIndex, string[] value)
        {
            lock (locker) ThreadPages[arrayIndex] = value;
        }
        public void AddToThreadPagesPageLocked
                (int arrayIndex, int pageIndex, string value)
        {
            lock (locker) ThreadPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeThreadPagesLocked(string[][] value)
        {
            lock (locker) ThreadPages = value;
        }
        public string[][] GetThreadPagesLocked()
        {
            lock (locker) return ThreadPages;
        }
        public int GetThreadPagesPageDepthLocked(int index)
        {
            lock (locker) return ThreadPagesPageDepth[index];
        }
        public void AddToThreadPagesPageDepthLocked(int index, int value)
        {
            lock (locker) ThreadPagesPageDepth[index] += value;
        }
        public void SetThreadPagesPageDepthLocked(int index, int value)
        {
            lock (locker) ThreadPagesPageDepth[index] = value;
        }
        public void InitializeThreadPagesPageDepthLocked(int[] value)
        {
            lock (locker) ThreadPagesPageDepth = value;
        }
        public int[] GetThreadPagesPageDepthLocked()
        {
            lock (locker) return ThreadPagesPageDepth;
        }
        public int GetThreadPagesLengthLocked()
        {
            lock (locker) return ThreadPagesLength;
        }
        public void SetThreadPagesLengthLocked(int value)
        {
            lock (locker) ThreadPagesLength = value;
        }
        public void InitializeMessagesToPublish()
        {
            MessagesToPublish = new ConcurrentQueue<MessageData>();
        }
        public void MessagesToPublishEnqueue(MessageData messageData)
        => MessagesToPublish.Enqueue(messageData);
        public void MessagesToPublishTryDequeue(out MessageData value)
        {
            MessagesToPublish.TryDequeue(out MessageData data);
            value = data;
        }
        public bool PreRegistrationLineTryAdd(int val, PreRegBag bag)
        => PreRegistrationLine.TryAdd(val, bag);
        public void InitializePreRegistrationLine()
        {
            PreRegistrationLine = new ConcurrentDictionary<int, PreRegBag>();
        }
        public void InitializeRegistrationLine()
        {
            RegistrationLine = new ConcurrentDictionary<int, RegBag>();
        }
        public void CaptchaMessagesRegistrationDataEnqueue(uint captchaHash)
        {
            CaptchaMessages_RegistrationData.Enqueue(captchaHash);
        }
        public void CaptchaMessagesRegistrationDataDequeue()
        => CaptchaMessages_RegistrationData.Dequeue();
        public void InitializeCaptchaMessagesRegistrationData()
        {
            CaptchaMessages_RegistrationData = new Queue<uint>(Constants.RegistrationPagesCount);
        }
        public bool CaptchaMessagesRegistrationDataContains(uint captcha)
        => CaptchaMessages_RegistrationData.Contains(captcha);
        public void RegistrationLineTryRemove(int i, out RegBag regBag)
        {
            RegistrationLine.TryRemove(i, out RegBag bag);
            regBag = bag;
        }
        public void PreRegistrationLineTryRemove(int key, out PreRegBag preRegBag)
        {
            PreRegistrationLine.TryRemove(key, out PreRegBag temp);
            preRegBag = temp;
        }
        public void RegistrationLineTryAdd(int val, RegBag regBag)
        => RegistrationLine.TryAdd(val, regBag);
        public void CaptchaMessagesEnqueue(uint captchaHash)
        => CaptchaMessages.Enqueue(captchaHash);
        public void InitializeCaptchaMessages()
        {
            CaptchaMessages = new Queue<uint>(Constants.LoginPagesCount);
        }
        public void CaptchaMessagesDequeue()
        => CaptchaMessages.Dequeue();
        public bool CaptchaMessagesContains(uint captchaHash)
        => CaptchaMessages.Contains(captchaHash);
        public bool LoginPasswordHashesValuesContains(Guid guid)
        => LoginPasswordHashes.Values.Contains(guid);
        public void SetLoginPasswordHashesPairToken(Pair pair, Guid? token)
        {
            LoginPasswordHashes[pair] = token;
        }
        public Guid? LoginPasswordHashesToken(Pair key)
        => LoginPasswordHashes[key];
    }
}