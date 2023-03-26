using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using XXHash;
using System.Net;
using static Data.DataLockers.Lockers;
namespace Data
{ // Перекрёстные ссылки между методами исключены.
    public sealed partial class Memory
    { //TODO разбить на локеры
        public bool LoginPasswordAccIdHashesContainsKey(Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashes.ContainsKey(pair);
        }
        public void LoginPasswordAccIdHashesTryAdd(Pair pair, int accountId)
        {
            lock (LoginPasswordAccIdHashesLocker)
                LoginPasswordAccIdHashes.TryAdd(pair, accountId);
        }
        public void LoginPasswordHashesDeltaTryRemove(Pair pair, out byte result)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDelta.TryRemove(pair, out result);
        }
        public void LoginPasswordHashesDeltaTryAdd(Pair pair, byte val)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDelta.TryAdd(pair, Constants.Zero);
        }
        public bool LoginPasswordHashesDeltaContainsKey(Pair pair)
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDelta.ContainsKey(pair);
        }
        public int GetLoginPasswordAccIdHashes(Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashes[pair];
        }
        public void InitializeLoginPasswordAccIdHashes()
        {
            lock (LoginPasswordAccIdHashesLocker)
                Memory.LoginPasswordAccIdHashes = new ConcurrentDictionary<Pair, int>();
        }
        public void InitializeLoginPasswordHashes()
        {
            lock (LoginPasswordHashesLocker)
                Memory.LoginPasswordHashes = new ConcurrentDictionary<Pair, Guid?>();
        }
        public IEnumerable<Pair> GetLoginPasswordHashesDeltaKeys()
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDelta.Keys;
        }
        public void InitializeLoginPasswordHashesDelta()
        {
            lock (LoginPasswordHashesDeltaLocker)
                Memory.LoginPasswordHashesDelta = new ConcurrentDictionary<Pair, byte>();
        }
        public void InitializePrivateMessages()
        {
            lock (PersonalPagesLocker)
            {
                Memory.PersonalPages = new
                    ConcurrentDictionary
                        <OwnerId, Dictionary<CompanionId, PrivateMessages>>();

                lock (PersonalPagesDepthsLocker)
                    Memory.PersonalPagesDepths = new
                    ConcurrentDictionary<OwnerId, Dictionary<CompanionId, int>>();
            }
        }
        public bool LoginPasswordHashesContainsKey(Pair pair)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashes.ContainsKey(pair);
        }
        public void LoginPasswordHashesTryAdd(Pair pair, Guid? guid)
        => LoginPasswordHashes.TryAdd(pair, null);
        public bool NicksHashesKeysContains(uint hash)
        {
            lock (NicksHashesLocker)
                return NicksHashes.ContainsKey(hash);
        }
        public void InitializeNicksHashes()
        {
            lock (NicksHashesLocker)
                NicksHashes = new ConcurrentDictionary<uint, byte>();
        }
        public void NicksHashesTryAdd(uint nickHash, byte temp)
        {
            lock (NicksHashesLocker)
                NicksHashes.TryAdd(nickHash, temp);
        }
        public string GetEndPointPageLocked(int index)
        {
            lock (EndPointPagesLocker)
                return EndPointPages[index];
        }
        public void SetEndPointPageLocked(int index, string value)
        {
            lock (EndPointPagesLocker)
                EndPointPages[index] = value;
        }
        public void InitializeEndPointPagesLocked(int size)
        {
            lock (EndPointPagesLocker)
                EndPointPages = new string[size];
        }
        public string GetMainContentLocked()
        {
            lock (MainContentLocker)
                return MainContent;
        }
        public void SetMainContentLocked(string value)
        {
            lock (MainContentLocker)
                MainContent = value;
        }
        public string GetMainPageLocked()
        {
            lock (MainPageLocker)
                return MainPage;
        }
        public void SetMainPageLocked(string value)
        {
            lock (MainPageLocker)
                MainPage = value;
        }
        public void AddToMainPageLocked(string value)
        {
            lock (MainPageLocker)
                MainPage += value;
        }
        public void DialogsToStartEnqueue(DialogData value)
        {
            lock (DialogsToStartLocker)
                DialogsToStart.Enqueue(value);
        }
        public void InitializeDialogsToStart()
        {
            lock (DialogsToStartLocker)
                DialogsToStart = new ConcurrentQueue<DialogData>();
        }
        public void DialogsToStartTryDequeue(out DialogData value)
        {
            lock (DialogsToStartLocker)
                DialogsToStart.TryDequeue(out value);
        }
        public void InitializePersonalMessagesToPublish()
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublish = new ConcurrentQueue<MessageData>();
        }
        public void PersonalMessagesToPublishEnqueue(MessageData value)
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublish.Enqueue(value);
        }
        public void PersonalMessagesToPublishTryDequeue(out MessageData value)
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublish.TryDequeue(out value);
        }
        public string[] GetDialogPagesArrayLocked(int index)
        {
            lock (DialogPagesLocker)
                return DialogPages[index];
        }
        public void SetDialogPagesArrayLocked(int index, string[] value)
        {
            lock (DialogPagesLocker)
                DialogPages[index] = value;
        }
        public string GetDialogPagesPageLocked(int arrayIndex, int pageIndex)
        {
            lock (DialogPagesLocker)
                return DialogPages[arrayIndex][pageIndex];
        }
        public void SetDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (DialogPagesLocker)
                DialogPages[arrayIndex][pageIndex] = value;
        }
        public void AddToDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (DialogPagesLocker)
                DialogPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeDialogPagesLocked(string[][] value)
        {
            lock (DialogPagesLocker)
                DialogPages = value;
        }
        public int GetDialogPagesLengthLocked()
        {
            lock (DialogPagesLengthLocker)
                return DialogPagesLength;
        }
        public void CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked()
        {
            lock (DialogPagesLocker)
            {
                lock (DialogPagesPageDepthLocker)
                {
                    lock (DialogPagesLengthLocker)
                    {
                        int len = DialogPagesLength;
                        DialogPagesLength++;
                        string[][] temp = new string[DialogPagesLength][];
                        Array.Copy(DialogPages, temp, len);
                        DialogPages = temp;
                        DialogPages[len] = new string[Constants.One] { Constants.NewDialog };
                        int[] temp1 = new int[DialogPagesLength];
                        Array.Copy(DialogPagesPageDepth, temp1, len);
                        DialogPagesPageDepth = temp1;
                        DialogPagesPageDepth[len] = Constants.One;
                    }
                }
            }
        }
        public void SetDialogPagesLengthLocked(int value)
        {
            lock (DialogPagesLengthLocker)
                DialogPagesLength = value;
        }
        public void SetDialogPagesPageDepthLocked(int index, int value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepth[index] = value;
        }
        public int GetDialogPagesPageDepthLocked(int index)
        {
            lock (DialogPagesPageDepthLocker)
                return DialogPagesPageDepth[index];
        }
        public void InitializeDialogPagesPageDepthLocked(int[] value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepth = value;
        }
        public string GetMessage(int ownerId, int companionId, int messageId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages
                              [new OwnerId { Id = ownerId }]
                             [new CompanionId { Id = companionId }]
                             .Messages[messageId];
        }
        public string[] GetMessages(int ownerId, int companionId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages
                    [new OwnerId { Id = ownerId }]
                    [new CompanionId { Id = companionId }]
                    .Messages;
        }
        public void AddToPersonalPagesDepth(int id, int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepths
                 [new OwnerId { Id = accountId }]
                 [new CompanionId { Id = id }]
                 ++;
        }
        public int GetPersonalPagesDepth(int id, int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = id }];
        }
        public void SetPersonalPagesPage
                (int id, int accountId, int depth, string page)
        {
            lock (PersonalPagesLocker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            .Messages[depth] = page;
        }
        public void SetPersonalPagesMessagesArray
                    (int id, int accountId, string[] value)
        {
            lock (PersonalPagesLocker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            = new PrivateMessages { Messages = value };
        }
        public bool PersonalPagesContainsKey(OwnerId ownerId, CompanionId companionId, bool flag)
        {
            lock (PersonalPagesLocker)
                return flag && PersonalPages[ownerId].ContainsKey(companionId);
        }
        public void PersonalPagesAdd(OwnerId ownerId, CompanionId companionId, string[] newMsg, bool flag)
        {
            lock (PersonalPagesLocker)
            {
                if (!flag)
                    PersonalPages.TryAdd(ownerId, new Dictionary<CompanionId, PrivateMessages>());
                PersonalPages[ownerId].Add(companionId,
                        new PrivateMessages { Messages = newMsg });
            }
        }
        public bool PersonalPagesKeysContains(OwnerId ownerId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages.ContainsKey(ownerId);
        }
        public bool PersonalPagesDepthsContainsKey(OwnerId ownerId, CompanionId companionId, bool flag)
        {
            lock (PersonalPagesDepthsLocker)
                return flag && PersonalPagesDepths[ownerId].ContainsKey(companionId);
        }
        public void PersonalPagesDepthsAdd(OwnerId ownerId, CompanionId companionId, bool flag)
        {
            lock (PersonalPagesDepthsLocker)
            {
                if (!flag)
                    PersonalPagesDepths.TryAdd(ownerId, new Dictionary<CompanionId, int>());
                PersonalPagesDepths[ownerId].Add(companionId, Constants.One);
            }
        }
        public bool PersonalPagesDepthsKeysContains(OwnerId ownerId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths.ContainsKey(ownerId);
        }
        public int GetPersonalPagesPageDepth(int accountId, int companionId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = companionId }];//проверить границы 
        }
        public void PersonalPagesTryAdd(OwnerId ownerId,
                                         Dictionary<CompanionId, PrivateMessages> temp1)
        {
            lock (PersonalPagesLocker)
                PersonalPages.TryAdd(ownerId, temp1);
        }
        public void PersonalPagesDepthsTryAdd(OwnerId ownerId,
                                                Dictionary<CompanionId, int> temp2)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepths.TryAdd(ownerId, temp2);
        }
        public string[] GetSectionPagesArrayLocked(int index)
        {
            lock (SectionPagesLocker)
                return SectionPages[index];
        }
        public void SetSectionPagesArrayLocked(int index, string[] value)
        {
            lock (SectionPagesLocker)
                SectionPages[index] = value;
        }
        public string GetSectionPagesPageLocked(int arrayIndex, int pageIndex)
        {
            lock (SectionPagesLocker)
                return SectionPages[arrayIndex][pageIndex];
        }
        public void SetSectionPagesPageLocked(int arrayIndex, int pageIndex, string value)
        {
            lock (SectionPagesLocker)
                SectionPages[arrayIndex][pageIndex] = value;
        }
        public void AddToSectionPagesPageLocked(int arrayIndex, int pageIndex, string value)
        {
            lock (SectionPagesLocker)
                SectionPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeSectionPagesLocked(string[][] value)
        {
            lock (SectionPagesLocker)
                SectionPages = value;
        }
        public int GetSectionPagesLengthLocked()
        {
            lock (SectionPagesLengthLocker)
                return SectionPagesLength;
        }
        public void SetSectionPagesLengthLocked(int value)
        {
            lock (SectionPagesLengthLocker)
                SectionPagesLength = value;
        }
        public void SetSectionPagesPageDepthLocked(int index, int value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepth[index] = value;
        }
        public int GetSectionPagesPageDepthLocked(int index)
        {
            lock (SectionPagesPageDepthLocker)
                return SectionPagesPageDepth[index];
        }
        public void InitializeSectionPagesPageDepthLocked(int[] value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepth = value;
        }
        public void InitializeTopicsToStart()
        {
            lock (TopicsToStartLocker)
                TopicsToStart = new ConcurrentQueue<TopicData>();
        }
        public void SetSectionPagesArray(int endpointId)
        {
            lock (SectionPagesLocker)
                lock (pagesLocker)
                    pages = SectionPages[endpointId - Constants.One];
        }
        public int GetPagesLength()
        {
            lock (pagesLocker)
                return pages.Length;
        }
        public string GetLastPage()
        {
            lock (pagesLocker)
                return pages[pages.Length - Constants.One];
        }
        public int GetPreRegistrationLineCount()
        {
            lock (PreRegistrationLineLocker)
                return PreRegistrationLine.Count;
        }
        public int GetCaptchaMessagesRegistrationDataCount()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                return CaptchaMessages_RegistrationData.Count;
        }
        public string GetCaptchaPageToReturn()
        {
            lock (CaptchaPageToReturnLocker)
                return CaptchaPageToReturn;
        }
        public string GetPageToReturnRegistrationData()
        {
            lock (PageToReturn_RegistrationDataLocker)
                return PageToReturn_RegistrationData;
        }
        public int GetPos()
        {
            lock (posLocker)
                return pos;
        }
        public string GetTemp()
        {
            lock (tempLocker)
                return temp;
        }
        public void SetTemp(string value)
        {
            lock (tempLocker)
                temp = value;
        }
        public void SetPos(int value)
        {
            lock (posLocker)
                pos = value;
        }
        public void SetPageToReturnRegistrationData(string value)
        {
            lock (PageToReturn_RegistrationDataLocker)
                PageToReturn_RegistrationData = value;
        }
        public void SetCaptchaPageToReturn(string value)
        {
            lock (CaptchaPageToReturnLocker)
                CaptchaPageToReturn = value;
        }
        public int GetCaptchaMessagesCount()
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessages.Count;
        }
        public int GetRegistrationLineCount()
        {
            lock (RegistrationLineLocker)
                return RegistrationLine.Count;
        }
        public int GetPersonalMessagesToPublishCount()
        {
            lock (PersonalMessagesToPublishLocker)
                return PersonalMessagesToPublish.Count;
        }
        public int GetDialogsToStartCount()
        {
            lock (DialogsToStartLocker)
                return DialogsToStart.Count;
        }
        public int GetMessagesToPublishCount()
        {
            lock (MessagesToPublishLocker)
                return MessagesToPublish.Count;
        }
        public int GetTopicsToStartCount()
        {
            lock (TopicsToStartLocker)
                return TopicsToStart.Count;
        }
        public int GetThreadsCount()
        {
            lock (threadsCountLocker)
                return threadsCount;
        }
        public string[] GetPages()
        {
            lock (pagesLocker)
                return pages;
        }
        public void SetThreadsCount(int threadsCount)
        {
            lock (threadsCountLocker)
                Memory.threadsCount = threadsCount;
        }
        public void SetPages(string[] temp)
        {
            lock (pagesLocker)
                pages = temp;
        }
        public bool SpecialSearch(char c)
        {
            lock (SpecialLocker)
            {
                byte i = 0;

                while (Constants.Special[i] != '\0')
                {
                    if (Constants.Special[i] == c)
                        return true;
                    else
                        i++;
                }

                return false;
            }
        }
        public void SetLastPage(string value)
        {
            lock (pagesLocker)
                pages[pages.Length - Constants.One] = value;
        }
        public string GetPage(int num)
        {
            lock (pagesLocker)
                return pages[num];
        }
        public void SetPage(int index, string value)
        {
            lock (pagesLocker)
                pages[index] = value;
        }
        public void TopicsToStartEnqueue(TopicData value)
        {
            lock (TopicsToStartLocker)
                TopicsToStart.Enqueue(value);
        }
        public void TopicsToStartTryDequeue(out TopicData topicData)
        {
            lock (TopicsToStartLocker)
            {
                TopicsToStart.TryDequeue(out TopicData data);
                topicData = data;
            }
        }
        public void SetThreadPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex][pageIndex] = value;
        }
        public string GetThreadPagesPageLocked
            (int arrayIndex, int pageIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPages[arrayIndex][pageIndex];
        }
        public string[] GetThreadPagesArrayLocked(int arrayIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPages[arrayIndex];
        }
        public void SetThreadPagesArrayLocked
                (int arrayIndex, string[] value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex] = value;
        }
        public void AddToThreadPagesPageLocked
                (int arrayIndex, int pageIndex, string value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeThreadPagesLocked(string[][] value)
        {
            lock (ThreadPagesLocker)
                ThreadPages = value;
        }
        public string[][] GetThreadPagesLocked()
        {
            lock (ThreadPagesLocker)
                return ThreadPages;
        }
        public int GetThreadPagesPageDepthLocked(int index)
        {
            lock (ThreadPagesPageDepthLocker)
                return ThreadPagesPageDepth[index];
        }
        public void AddToThreadPagesPageDepthLocked(int index, int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth[index] += value;
        }
        public void SetThreadPagesPageDepthLocked(int index, int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth[index] = value;
        }
        public void InitializeThreadPagesPageDepthLocked(int[] value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth = value;
        }
        public int[] GetThreadPagesPageDepthLocked()
        {
            lock (ThreadPagesPageDepthLocker)
                return ThreadPagesPageDepth;
        }
        public int GetThreadPagesLengthLocked()
        {
            lock (ThreadPagesLengthLocker)
                return ThreadPagesLength;
        }
        public void SetThreadPagesLengthLocked(int value)
        {
            lock (ThreadPagesLengthLocker)
                ThreadPagesLength = value;
        }
        public void InitializeMessagesToPublish()
        {
            lock (MessagesToPublishLocker)
                MessagesToPublish = new ConcurrentQueue<MessageData>();
        }
        public void MessagesToPublishEnqueue(MessageData messageData)
        {
            lock (MessagesToPublishLocker)
                MessagesToPublish.Enqueue(messageData);
        }
        public void MessagesToPublishTryDequeue(out MessageData value)
        {
            lock (MessagesToPublishLocker)
            {
                MessagesToPublish.TryDequeue(out MessageData data);
                value = data;
            }
        }
        public bool PreRegistrationLineTryAdd(int val, PreRegBag bag)
        {
            lock (PreRegistrationLineLocker)
                return PreRegistrationLine.TryAdd(val, bag);
        }
        public void InitializePreRegistrationLine()
        {
            lock (PreRegistrationLineLocker)
                PreRegistrationLine = new ConcurrentDictionary<int, PreRegBag>();
        }
        public void InitializeRegistrationLine()
        {
            lock (RegistrationLineLocker)
                RegistrationLine = new ConcurrentDictionary<int, RegBag>();
        }
        public void CaptchaMessagesRegistrationDataEnqueue(uint captchaHash)
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData.Enqueue(captchaHash);
        }
        public void CaptchaMessagesRegistrationDataDequeue()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData.Dequeue();
        }
        public void InitializeCaptchaMessagesRegistrationData()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData = new Queue<uint>(Constants.RegistrationPagesCount);
        }
        public bool CaptchaMessagesRegistrationDataContains(uint captcha)
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                return CaptchaMessages_RegistrationData.Contains(captcha);
        }
        public void RegistrationLineTryRemove(int i, out RegBag regBag)
        {
            lock (RegistrationLineLocker)
            {
                RegistrationLine.TryRemove(i, out RegBag bag);
                regBag = bag;
            }
        }
        public void PreRegistrationLineTryRemove(int key, out PreRegBag preRegBag)
        {
            lock (PreRegistrationLineLocker)
            {
                PreRegistrationLine.TryRemove(key, out PreRegBag temp);
                preRegBag = temp;
            }
        }
        public void RegistrationLineTryAdd(int val, RegBag regBag)
        {
            lock (RegistrationLineLocker)
                RegistrationLine.TryAdd(val, regBag);
        }
        public void CaptchaMessagesEnqueue(uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages.Enqueue(captchaHash);
        }
        public void InitializeCaptchaMessages()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages = new Queue<uint>(Constants.LoginPagesCount);
        }
        public void CaptchaMessagesDequeue()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages.Dequeue();
        }
        public bool CaptchaMessagesContains(uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessages.Contains(captchaHash);
        }
        public bool LoginPasswordHashesValuesContains(Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashes.Values.Contains(guid);
        }
        public void SetLoginPasswordHashesPairToken(Pair pair, Guid? token)
        {
            lock (LoginPasswordHashesLocker)
                LoginPasswordHashes[pair] = token;
        }
        public bool IncrementWithValueRemoteIpHashesAttemptsCountersAndGrantAccessAndAddIfNotPresented(IPAddress ipAddress, byte value)
        {
            uint ipHash;

            if (ipAddress == null)
                return false;
            else
                ipHash = XXHash32.Hash(ipAddress.ToString());

            lock (RemoteIpHashesAttemptsCounterLocker)
            {
                if (!RemoteIpHashesAttemptsCounter.ContainsKey(ipHash))
                    RemoteIpHashesAttemptsCounter.Add(ipHash, value);
                short sum = (short)(value + RemoteIpHashesAttemptsCounter[ipHash]);

                if (sum > Constants.MaxAttemptsCountPerIp)
                {
                    RemoteIpHashesAttemptsCounter[ipHash] = Constants.MaxAttemptsCountPerIp;

                    return false;
                }
                else
                {
                    RemoteIpHashesAttemptsCounter[ipHash] = (byte)sum;

                    return true;
                }
            }
        }
        public void DecrementAllRemoteIpHashesAttemptsCountersAndRemoveUnnecessaryByTimer()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                foreach (var pair in RemoteIpHashesAttemptsCounter)
                    if (pair.Value > Constants.One)
                        RemoteIpHashesAttemptsCounter[pair.Key] = (byte)(pair.Value - Constants.One);
                    else
                        RemoteIpHashesAttemptsCounter.Remove(pair.Key);
        }
        public void InitializeRemoteIpHashesAttemptsCounter()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                RemoteIpHashesAttemptsCounter = new Dictionary<uint, byte>();
        }
        public void LoginPasswordHashesThroughIterationCheck(ref Pair pair, Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                if (LoginPasswordHashes.Values.Contains(guid))
                    foreach (var key in LoginPasswordHashes.Keys)
                        if (LoginPasswordHashes[key] == guid)
                        {
                            pair = key;

                            break;
                        }
        }
    }
}