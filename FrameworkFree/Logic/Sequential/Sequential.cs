
namespace Logic
{
    internal sealed class Sequential : ISequential // реализует методы, выполняемые последовательно и не требующие дополнительной блокировки
    {
        public IUnstableSequential Unstable { get; } // доступ к содержимому производится только через эти два свойства (между модулями тоже)
        public IStableSequential Stable { get; }

        public Sequential(IUnstableSequential unstable, IStableSequential stable)
        {
            Unstable = unstable;
            Stable = stable;
        }
    }
}