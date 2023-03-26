using System.Collections.Generic;
namespace Logic
{
    public sealed class AuthenticationUnstable : IAuthenticationUnstable
    {
        private readonly IStorage Storage;

        public AuthenticationUnstable(IStorage storage)
        {
            Storage = storage;
        }

        public void FlushAccountIdentifierRemoteIpLogByTimer()
        {
            Queue<AccountIdentifierRemoteIp> set = Storage.Fast.GetAccountIdentifierRemoteIps();

            foreach (var pair in set)
                Storage.Slow.PutAccountIdentifierIpHashInBaseIfNotExists(pair.AccountIdentifier, pair.RemoteIp);
            Storage.Fast.ClearAccountIdentifierRemoteIps();
        }
    }
}