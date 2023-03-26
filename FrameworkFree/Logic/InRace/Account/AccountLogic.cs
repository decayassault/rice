using MarkupHandlers;
using XXHash;
namespace Logic
{
    internal sealed partial class AccountLogic : IAccountLogic
    {
        private readonly IStorage Storage;
        private readonly ProfileMarkupHandler ProfileMarkupHandler;
        public AccountLogic(IStorage storage,
        ProfileMarkupHandler profileMarkupHandler)
        {
            Storage = storage;
            ProfileMarkupHandler = profileMarkupHandler;
        }

        public Pair? CheckPair(in string login, in string password)
        {
            var pair = new Pair
            { LoginHash = XXHash32.Hash(login), PasswordHash = XXHash32.Hash(password) };

            if (Storage.Fast.LoginPasswordHashesContainsKey(pair))
                return pair;
            else
                return null;
        }
    }
}
