namespace GameEngine.Common.Interfaces
{
    public interface IEventManager
    {
        bool HasEvent<T>();
        void AddListener<T>(Action<T> handler);
        void RemoveListener<T>(Action<T> handler);
        void Publish<T>(T data);
        void Clear();
    }
}
