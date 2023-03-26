namespace Logic
{
    public interface IInRace // обращения к многопоточным операциям только через этот интерфейс (между модулями тоже)
    {
        public IUnstableInRace Unstable { get; } // непротестированный функционал
        public IStableInRace Stable { get; } // протестированный функционал
    }
}