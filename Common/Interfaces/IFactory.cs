namespace Common.Interfaces
{
    public interface IFactory<T>
    {
        public T Create();
    }
}
