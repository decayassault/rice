using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Models;
namespace Data
{
    public sealed partial class Database
    {
        public int GetAccountsCount()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Count();
        }
        public void RemoveAccountByNickIfExists(string nick)
        {
            using (var bag = new TotalForumDbContext())
            {
                if (bag.Account.AsNoTracking().Count(account => account.Nick == nick) == 1)
                {
                    bag.Account.Remove(new Account { Nick = nick });
                    bag.SaveChanges();
                }
            }
        }
        public IList<Pair> GetPairs()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.Select(a => new Pair { LoginHash = unchecked((uint)a.Identifier), PasswordHash = unchecked((uint)a.Passphrase) }).ToList();
        }
        public IEnumerable<string> GetNicks()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Select(a => a.Nick).ToList();
        }
        public int? GetAccountIdFromBase
                (uint loginHash, uint passwordHash)
        {
            int? result = null;

            using (var bag = new TotalForumDbContext())
            {
                Account temp = null;
                if (bag.Account != null)
                    temp = bag.Account.AsNoTracking().FirstOrDefault(unchecked(a => a.Identifier == unchecked((int)loginHash) && a.Passphrase == unchecked((int)passwordHash)));
                else
                { }

                if (temp != null)
                    result = temp.Id;
            }
            return result;
        }
        public IEnumerable<IdName> GetEndpointIdNames(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Endpoint.AsNoTracking().Where(e => e.ForumId == id).OrderBy(b => b.Id).Select(a =>
                   new IdName { Id = a.Id, Name = a.Name }).Take(5).ToList();
        }
        public IEnumerable<IdName> GetForumIdNames()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Forum.AsNoTracking()
                    .OrderBy(f => f.Id).Take(5)
                    .Select(a => new IdName { Id = a.Id, Name = a.Name }).ToList();
        }
        public bool CheckNickInBase(string nick)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Any(a => a.Nick == nick);
        }
        public int GetIdByNick(string nick)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().First(a => a.Nick == nick).Id;
        }
        public void PutPrivateMessageInBase
            (int senderAccId, int acceptorAccId, string privateText)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Add(new PrivateMessage()
                {
                    SenderAccountId = senderAccId,
                    AcceptorAccountId = acceptorAccId,
                    PrivateText = privateText
                }); //в БД должно быть хотя бы одно приватное сообщение
                bag.SaveChanges();
            }
        }
        public int PutThreadAndMessageInBase(Thread thread, int accountId, string message)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Thread.Add(thread);
                bag.SaveChanges();
                bag.Msg.Add(new Msg { ThreadId = thread.Id, AccountId = accountId, MsgText = message });
                bag.SaveChanges();
            }

            return thread.Id;
        }
        public int CountPrivateMessages(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking().Where(pm => pm.AcceptorAccountId == id).Select(i => new { Id = i.SenderAccountId })
                                              .Union(bag.PrivateMessage.AsNoTracking().Where(pm => pm.SenderAccountId == id).Select(i => new { Id = i.AcceptorAccountId }))
                               .Intersect(bag.Account.AsNoTracking().Select(a => new { Id = a.Id })) //в БД должен быть хотя бы один аккаунт
                               .Count();
        }
        public IEnumerable<IdText> GetPrivateMessagesByIds(int companionId, int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking()
                    .Where(pm => pm.SenderAccountId == accountId
                        && pm.AcceptorAccountId == companionId
                        || pm.SenderAccountId == companionId
                        && pm.AcceptorAccountId == accountId).OrderBy(i => i.Id)
                    .Select(m => new IdText
                    {
                        SenderAccountId = m.SenderAccountId,
                        PrivateText = m.PrivateText
                    }).ToList();
        }
        public void AddAccount(Account dbAccount)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Account.Add(dbAccount);
                bag.SaveChanges();
            }
        }
        public void PutMessageInBase(Msg dbMessage)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Msg.Add(dbMessage);
                bag.SaveChanges();
            }
        }
        public IEnumerable<IdNick> GetIdNicksByAccountId(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking()
                        .Join(bag.PrivateMessage.AsNoTracking()
                            .Where(m => m.AcceptorAccountId == id),
                                account => account.Id, pm => pm.SenderAccountId,
                                (account, pm) => new IdNick { Id = account.Id, Nick = account.Nick })
                                  .Union(bag.Account.AsNoTracking()
                                  .Join(bag.PrivateMessage.AsNoTracking()
                                  .Where(m => m.SenderAccountId == id), account => account.Id,
                                  pm => pm.AcceptorAccountId, (account, pm) =>
                                    new IdNick { Id = account.Id, Nick = account.Nick })).Distinct().ToList();
        }
        public CompanionId[] GetCompanionsByAccountId(int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Join(bag.PrivateMessage.AsNoTracking().Where(pm => pm.AcceptorAccountId == accountId),
                    account => account.Id, message => message.SenderAccountId, (account, message) => new CompanionId { Id = account.Id })
                    .Union(bag.Account.AsNoTracking().Join(bag.PrivateMessage.AsNoTracking().Where(pm => pm.SenderAccountId == accountId),
                    account => account.Id, message => message.AcceptorAccountId, (account, message) => new CompanionId { Id = account.Id })).Distinct().ToArray();

        }
        public int CountPrivateMessagesByIds(int companionId, int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking()
                    .Count(pm => pm.SenderAccountId == accountId
                    && pm.AcceptorAccountId == companionId
                    || pm.SenderAccountId == companionId
                    && pm.AcceptorAccountId == accountId);
        }
        public int CountThreadsById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().Count(t => t.EndpointId == id);
        }
        public IEnumerable<IdName> GetThreadIdNamesById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking()
                    .Where(t => t.EndpointId == id)
                    .Select(i => new IdName { Id = i.Id, Name = i.Name })
                    .OrderByDescending(j => j.Id).ToList();
        }
        public int CountMessagesByAmount(int amount)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Msg.AsNoTracking().Count(m => m.ThreadId == amount);
        }
        public IEnumerable<Message> GetMessagesByAmount(int amount)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Msg.AsNoTracking()
                    .Where(m => m.ThreadId == amount)
                    .Select(msg => new Message
                    {
                        Id = msg.Id,
                        AccountId = msg.AccountId,
                        MsgText = msg.MsgText
                    })
                    .OrderBy(ms => ms.Id).ToList();
        }
        public int GetSectionNumById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().First(t => t.Id == id).EndpointId;
        }
        public string GetThreadNameById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().First(t => t.Id == id).Name;
        }
        public string GetNickByAccountId(int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().First(a => a.Id == accountId).Nick;
        }
        public int GetThreadsCount()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().Count();
        }
    }
}