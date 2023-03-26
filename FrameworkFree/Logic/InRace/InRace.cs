namespace Logic
{
    internal sealed class InRace : IInRace // реализует методы, выполняемые с конкурентным доступом
    {
        public IUnstableInRace Unstable { get; } // доступ к содержимому производится только через эти два свойства (между модулями тоже)
        public IStableInRace Stable { get; }

        public InRace(IUnstableInRace unstable, IStableInRace stable)
        {
            Unstable = unstable;
            Stable = stable;
        }
    }
}