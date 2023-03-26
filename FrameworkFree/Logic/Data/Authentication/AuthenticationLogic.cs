using System;

namespace Data
{//убрать полный проход по словарю
    internal sealed class AuthenticationLogic : IAuthenticationLogic
    {
        private static readonly object locker = new object();
        public readonly IStorage Storage;
        public AuthenticationLogic(IStorage storage)
        {
            Storage = storage;
        }
        public bool AccessGranted(string token)
        {
            lock (locker)
            {
                Guid guid;
                bool result = false;

                try
                {
                    guid = new Guid(token);
                }
                catch (System.FormatException)
                {
                    guid = Guid.Empty;
                }

                if (guid == Guid.Empty)
                { }
                else
                    result = Storage.Fast.LoginPasswordHashesValuesContains(guid);

                return result;
            }
        }

        public string Accept(Pair pair)
        {
            lock (locker)
            {
                Guid token = Guid.NewGuid();
                Storage.Fast.SetLoginPasswordHashesPairToken(pair, token);

                return token.ToString();
            }
        }

        public Pair GetPair(string token)
        {
            lock (locker)
            {
                Guid guid;

                try
                {
                    guid = new Guid(token);
                }
                catch (System.FormatException)
                {
                    guid = Guid.Empty;
                }
                Pair result = new Pair();

                if (guid == Guid.Empty)
                { }
                else
                    Storage.Fast.LoginPasswordHashesThroughIterationCheck(ref result, guid);

                return result;
            }
        }
    }
}
