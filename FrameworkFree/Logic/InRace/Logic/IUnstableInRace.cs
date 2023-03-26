namespace Logic
{
    public interface IUnstableInRace
    {
        public IAccountLogic Account { get; }
        public IAuthenticationLogic Authentication { get; }
    }
}