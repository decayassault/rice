using System.Collections.Generic;
using System;
using System.Net;
using Models;
namespace Data
{
    public interface IDatabase //напрямую в базу данных обращаться нельзя
    {
        void RemoveAccountByNickIfExists(string nick);
        int GetAccountsCount();
        IList<Pair> GetPairs();
        void PutAccountIdentifierIpHashInBaseIfNotExists
            (uint accountIdentifierHash, IPAddress ip);
        IEnumerable<string> GetNicks();
        int? GetAccountIdFromBase(uint loginHash, uint passwordHash);
        IEnumerable<IdName> GetEndpointIdNames(int id);
        IEnumerable<IdName> GetForumIdNames();
        bool CheckNickInBase(string nick);
        int GetIdByNick(string nick);
        void CheckAccountId(Func<Pair, int?> getAccIdAndStoreSlow);
        void PutPrivateMessageInBase
            (int senderAccId, int acceptorAccId, string privateText);
        int PutThreadAndMessageInBase(Thread thread, int accountId, string message);
        int CountPrivateMessages(int id);
        IEnumerable<IdNick> GetIdNicksByAccountId(int id);
        CompanionId[] GetCompanionsByAccountId(int accountId);
        int CountPrivateMessagesByIds(int companionId, int accountId);
        IEnumerable<IdText> GetPrivateMessagesByIds(int companionId, int accountId);
        void AddAccount(Account dbAccount);
        void PutMessageInBase(Msg dbMessage);
        int CountThreadsById(int id);
        IEnumerable<IdName> GetThreadIdNamesById(int id);
        int CountMessagesByAmount(int amount);
        IEnumerable<Message> GetMessagesByAmount(int amount);
        int GetSectionNumById(int id);
        string GetThreadNameById(int id);
        string GetNickByAccountId(int accountId);
        int GetThreadsCount();
    }
}