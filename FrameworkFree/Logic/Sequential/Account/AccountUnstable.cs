using MarkupHandlers;
using Logic;
using System.Collections.Generic;
using System.Linq;
using XXHash;
namespace Logic
{
    internal sealed class AccountUnstable : IAccountUnstable
    {
        private readonly IStorage Storage;
        private readonly ProfileMarkupHandler ProfileMarkupHandler;

        public AccountUnstable(IStorage storage,
        ProfileMarkupHandler profileMarkupHandler)
        {
            Storage = storage;
            ProfileMarkupHandler = profileMarkupHandler;
        }

        public void CheckAccountIdByTimer()
        {
            Storage.Slow.CheckAccountId(GetAccIdAndStore);
        }
        public int? GetAccIdAndStore(Pair pair)
        {
            int? accountId = Storage.Slow.GetAccountIdFromBase
                            (pair.LoginHash, pair.PasswordHash);

            if (accountId.HasValue)
            {
                Storage.Fast.LoginPasswordAccIdHashesAdd(pair, accountId.Value);
                byte result;
                Storage.Fast.LoginPasswordHashesDeltaRemove(pair, out result);
                Storage.Fast.AddOrUpdateOwnProfilePage(accountId.Value,
                            ProfileMarkupHandler
                                .GetOwnProfilePrimaryUnfilledMarkup(accountId.Value));
            }

            return accountId;
        }

        public void LoadAccounts()
        {
            ProcessAccountsReader(Storage.Slow.GetPairs());
        }

        public void LoadNicks()
        {
            ProcessNicksReader(Storage.Slow.GetNicks());
        }

        public void ProcessAccountsReader(in ICollection<Pair> pairs)
        {
            Storage.Fast.InitializeLoginPasswordAccIdHashes();
            Storage.Fast.InitializeLoginPasswordHashes();
            Storage.Fast.InitializeLoginPasswordHashesDelta();

            if (pairs.Any())
            {
                foreach (Pair pair in pairs)
                {
                    if (pair.LoginHash != Constants.Zero && pair.PasswordHash != Constants.Zero)
                    {
                        if (!Storage.Fast.LoginPasswordAccIdHashesContainsKey(pair))
                        {
                            int? accountId = Storage.Slow.GetAccountIdFromBase
                                (pair.LoginHash, pair.PasswordHash);

                            if (accountId.HasValue)
                                Storage.Fast.LoginPasswordAccIdHashesAdd(pair, accountId.Value);
                        }

                        if (!Storage.Fast.LoginPasswordHashesContainsKey(pair))
                        {
                            Storage.Fast.LoginPasswordHashesAdd(pair, null);
                        }
                    }
                }
            }
        }

        public void ProcessNicksReader(in IEnumerable<string> nicks)
        {
            Storage.Fast.InitializeNicksHashes();

            if (nicks.Any())
                foreach (var nick in nicks)
                    Storage.Fast.NicksHashesAdd(XXHash32.Hash(nick), Constants.Zero);
        }

        public bool CheckNickHashIfExists(in string nick)
        {
            bool result = false;

            if (Storage.Fast.NicksHashesKeysContains(XXHash32.Hash(nick)))
                result = true;
            else
                result = false;

            return result;
        }
    }
}