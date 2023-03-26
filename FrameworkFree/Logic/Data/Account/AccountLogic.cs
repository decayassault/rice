using System.Collections.Generic;
using System.Linq;
namespace Data
{
    internal sealed class AccountLogic : IAccountLogic
    {
        public readonly IStorage Storage;
        public AccountLogic(IStorage storage)
        {
            Storage = storage;
        }
        private static readonly object locker = new object();

        public void LoadAccounts()
        {
            ProcessAccountsReader(Storage.Slow.GetPairs());
        }

        public void LoadNicks()
        {
            ProcessNicksReader(Storage.Slow.GetNicks());
        }

        public void CheckAccountIdByTimer()
        {
            Storage.Slow.CheckAccountId(GetAccIdAndStore);
        }

        public int? GetAccIdAndStore(Pair pair)
        {
            int? accountId = null;

            accountId = Storage.Slow.GetAccountIdFromBase
                            (pair.LoginHash, pair.PasswordHash);

            if (accountId.HasValue)
            {
                Storage.Fast.LoginPasswordAccIdHashesAdd(pair, accountId.Value);
                byte result;
                Storage.Fast.LoginPasswordHashesDeltaRemove(pair, out result);//проверить на коллизии
            }

            return accountId;
        }
        public void ProcessAccountsReader(IList<Pair> pairs)
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

        public Pair? CheckPair(string login, string password)
        {
            uint loginHash = XXHash.XXHash32.Hash(login);
            uint passwordHash = XXHash.XXHash32.Hash(password);
            var pair = new Pair
            { LoginHash = loginHash, PasswordHash = passwordHash };

            if (Storage.Fast.LoginPasswordHashesContainsKey(pair))
                return pair;
            else
                return null;
        }

        public bool CheckNickHashIfExists(string nick)
        {
            bool result = false;
            uint hash = XXHash.XXHash32.Hash(nick);

            if (Storage.Fast.NicksHashesKeysContains(hash))
                result = true;
            else
                result = false;

            return result;
        }

        public void ProcessNicksReader(IEnumerable<string> nicks)
        {
            Storage.Fast.InitializeNicksHashes();

            if (nicks.Any())
                foreach (var nick in nicks)
                    Storage.Fast.NicksHashesAdd(XXHash.XXHash32.Hash(nick), Constants.Zero);
        }
    }
}
