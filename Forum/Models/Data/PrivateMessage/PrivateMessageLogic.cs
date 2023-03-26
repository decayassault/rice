using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Data.Account;
namespace Forum.Data.PrivateMessage
{
    internal sealed class PrivateMessageLogic
    {
        private const string SE = "";
        internal struct OwnerId
        {
            internal int Id;
        }
        internal struct CompanionId
        {
            internal int Id;
        }
        internal struct PrivateMessages
        {
            internal string[] Messages;
        }
        private static 
            ConcurrentDictionary
                <OwnerId, Dictionary<CompanionId, PrivateMessages>> PersonalPages;
        private static
            ConcurrentDictionary
                <OwnerId, Dictionary<CompanionId, int>> PersonalPagesDepths;
        internal async static Task<string> GetPersonalPage
                            (int? id, int? page, string username)
        {
            if (page == null||id==null)
                return SE;
            else
            {                
                int index = await ReplyData.GetAccountId(username);
                int Id = (int)id;                
                int limit = GetPersonalPagesPageDepth(index, Id);
                if (page > MvcApplication.Zero
                        && page
                        <= limit)
                {                    
                    return PersonalPages
                        [new OwnerId { Id = index }]
                        [new CompanionId { Id = Id }]
                        .Messages[(int)page-MvcApplication.One];
                }
                else
                    return SE;
            }
        }
        internal static string[] GetPersonalPagesArray(int id, int accountId)
        {
            return PersonalPages
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = id }]
                .Messages;
        }
        internal static string GetLastPersonalPage
                            (int id,  int accountId)
        {         
           return PersonalPages
                        [new OwnerId { Id = accountId }]
                        [new CompanionId { Id = id }]
                        .Messages[GetPersonalPagesPageDepth(accountId, id)
                        - MvcApplication.One];     
        }
        internal static void AddToPersonalPagesDepth(int id,int accountId)
        {
            PersonalPagesDepths
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = id }]
                ++;
        }
        internal static int GetPersonalPagesDepth(int id,int accountId)
        {
            return PersonalPagesDepths
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = id }];
        }
        internal static void SetPersonalPagesPage
                (int id, int accountId, int depth, string page)
        {
            PersonalPages
                        [new OwnerId { Id = accountId }]
                        [new CompanionId { Id = id }]
                        .Messages[depth] = page;  
        }
        internal static void SetPersonalPagesMessagesArray
                    (int id,int accountId,string[] value)
        {
            PersonalPages
                        [new OwnerId { Id = accountId }]
                        [new CompanionId { Id = id }]
                        = new PrivateMessages { Messages = value };  
        }
        internal static void AllowPrivateMessages()
        {
            PersonalPages = new 
                ConcurrentDictionary
                    <OwnerId,Dictionary<CompanionId,PrivateMessages>>();
            PersonalPagesDepths = new
            ConcurrentDictionary<OwnerId, Dictionary<CompanionId, int>>();
        }
        internal async static Task LoadPersonalPages() 
        {      
            await LoadEachPersonalPage();      
        }
        internal static void LoadPersonalPagesNoAsync()
        {
            LoadEachPersonalPageNoAsync();
        }
        internal async static Task LoadEachPersonalPage()
        {
            int ownersCount = await AccountData.GetAccountsCount();
            /*for (int i = 0; i < ownersCount; i++)
            {
                SetMessagesDictionary(i+1);                
            }           */
            //<test>
            for (int i = 0; i < ownersCount; i++)
            {
                SetMessagesDictionary(i + 1);
            }         
            //</test>
        }
        internal static void LoadEachPersonalPageNoAsync()
        {
            int ownersCount = AccountData.GetAccountsCountNoAsync();
            /*for (int i = 0; i < ownersCount; i++)
            {
                SetMessagesDictionary(i+1);                
            }           */
            //<test>
            for (int i = 0; i < ownersCount; i++)
            {
                SetMessagesDictionaryNoAsync(i + 1);
            }
            //</test>
        }
        private static int GetPersonalPagesPageDepth(int accountId,int companionId)
        {
            return PersonalPagesDepths
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = companionId }];
        }
        private async static void SetMessagesDictionary(int accountId)
        {
            CompanionId[] companions 
                = await PrivateMessageData.GetCompanions(accountId);
            
            Dictionary<CompanionId, PrivateMessages>
                    temp1 = new Dictionary<CompanionId, PrivateMessages>();
            Dictionary<CompanionId, int>
                    temp2 = new Dictionary<CompanionId, int>();
            /*for (int i = 0; i < companions.Length;i++ )
            {
                PrivateMessages pm = 
                    await PrivateMessageData
                            .GetMessages(companions[i].Id, accountId);
               
                temp1.Add(companions[i], pm);
                temp2.Add(companions[i],pm.Messages.Length);               
            }*/
            //<test>
            for (int i = 0; i < 1; i++)
            {
                PrivateMessages pm =
                        await PrivateMessageData
                                .GetMessages(companions[i].Id, accountId);

                temp1.Add(companions[i], pm);
                //temp2.Add(companions[i], pm.Messages.Length);
            }
            //</test>
            PersonalPages.TryAdd(new OwnerId { Id = accountId }, temp1);
            PersonalPagesDepths.TryAdd(new OwnerId { Id = accountId }, temp2);
        }
        private static void SetMessagesDictionaryNoAsync(int accountId)
        {
            CompanionId[] companions
                = PrivateMessageData.GetCompanionsNoAsync(accountId);

            Dictionary<CompanionId, PrivateMessages>
                    temp1 = new Dictionary<CompanionId, PrivateMessages>();
            Dictionary<CompanionId, int>
                    temp2 = new Dictionary<CompanionId, int>();
            for (int i = 0; i < companions.Length; i++)
            {
                PrivateMessages pm =
                        PrivateMessageData
                                .GetMessagesNoAsync
                                    (companions[i].Id, accountId);

                temp1.Add(companions[i], pm);
                temp2.Add(companions[i], pm.Messages.Length);
            }          
            PersonalPages.TryAdd(new OwnerId { Id = accountId }, temp1);
            PersonalPagesDepths.TryAdd(new OwnerId { Id = accountId }, temp2);
        }
    }
}