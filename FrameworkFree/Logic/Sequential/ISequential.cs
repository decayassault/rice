namespace Logic
{
    public interface ISequential
    {
        public IUnstableSequential Unstable { get; } // непротестированный функционал
        public IStableSequential Stable { get; } // протестированный функционал
    }
}