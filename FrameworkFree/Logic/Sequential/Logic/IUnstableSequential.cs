namespace Logic
{
    public interface IUnstableSequential
    {
        public IAccountUnstable Account { get; }
        public IAuthenticationUnstable Authentication { get; }
    }
}