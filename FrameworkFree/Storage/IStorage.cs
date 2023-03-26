namespace Data
{
    public interface IStorage
    {
        IMemory Fast { get; }
        IDatabase Slow { get; }
    }
}