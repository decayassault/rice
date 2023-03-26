namespace Logic
{
    internal sealed partial class UnstableSequential : IUnstableSequential
    {
        public IAccountUnstable Account { get; }
        public IAuthenticationUnstable Authentication { get; }

        public UnstableSequential(IAccountUnstable account, IAuthenticationUnstable authentication)
        {
            Account = account;
            Authentication = authentication;
        }
    }
}