namespace Logic
{
    public sealed class UnstableInRace : IUnstableInRace
    {
        public IAccountLogic Account { get; }
        public IAuthenticationLogic Authentication { get; }

        public UnstableInRace(IAccountLogic account, IAuthenticationLogic authentication)
        {
            Account = account;
            Authentication = authentication;
        }
    }
}