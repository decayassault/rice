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
        internal static 
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
                            (int companionId,  int ownerId)
        {
            bool test = PersonalPages[new OwnerId { Id = ownerId }]
                .ContainsKey(new CompanionId { Id = companionId });
            bool test1 = PersonalPagesDepths[new OwnerId { Id = ownerId }]
                .ContainsKey(new CompanionId { Id = companionId });
           return PersonalPages
                        [new OwnerId { Id = ownerId }]
                        [new CompanionId { Id = companionId }]
                        .Messages[GetPersonalPagesPageDepth(ownerId, companionId)
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
           
            for (int i = 0; i < ownersCount; i++)
            {
                SetMessagesDictionaryNoAsync(i + 1);
            }
           
        }
        internal static void AddNewCompanionsIfNotExists
                    (int ownerId,int companionId,string ownerNick,
            string companionNick)
        {
            OwnerId ownerIdObj=new OwnerId{Id=ownerId};
            CompanionId companionIdObj=new CompanionId{Id=companionId};
            
            SetNewCompanionDepth(ownerIdObj, companionIdObj);
            SetNewCompanionPage(ownerIdObj, companionIdObj,companionNick);
           
            ownerIdObj.Id = companionId;
            companionIdObj.Id = ownerId;
            
            SetNewCompanionDepth(ownerIdObj, companionIdObj);
            SetNewCompanionPage(ownerIdObj, companionIdObj,ownerNick);           
        }
        private static void SetNewCompanionPage
            (OwnerId ownerId, CompanionId companionId,string companionNick)
        {
            if(!PersonalPages[ownerId].ContainsKey(companionId))
            {
                string[]newMsg=new string[MvcApplication.One]
                {"<div class='s'>"+companionId.Id.ToString()+"</div>"+
                    "<div class='l'><h2 onclick='g(&quot;/dialog/1&quot;);'>Переписка с "+
                    companionNick+"</h2>"+
                    "<div id='a'><a onclick='replyPM();return false'>Ответить "
                    +companionNick+"</a></div></div><div class='s'>5</div>"
                };
                PersonalPages[ownerId].Add(companionId,
                    new PrivateMessages { Messages = newMsg });
            }
        }
        private static void SetNewCompanionDepth
            (OwnerId ownerId,CompanionId companionId)
        {
            if (!PersonalPagesDepths[ownerId]
                .ContainsKey(companionId))
                PersonalPagesDepths[ownerId]
                    .Add(companionId, MvcApplication.One);
        }
        private static int GetPersonalPagesPageDepth(int accountId,int companionId)
        {
            Dictionary<CompanionId, int> test1 = PersonalPagesDepths
                [new OwnerId { Id = accountId }];
            bool test2 = test1.ContainsKey(new CompanionId { Id = companionId });
            
            return PersonalPagesDepths
                [new OwnerId { Id = accountId }]
                [new CompanionId { Id = companionId }];//проверить границы
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