using Common.Interfaces;

namespace GameEngine.Core.Services
{
    public class EventManager : IEventManager
    {
        private readonly object _lockObject = new object();

        private Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

        public bool HasEvent<T>()
        {
            lock (_lockObject)
            {
                return _eventListeners.ContainsKey(typeof(T));
            }
        }

        public void AddListener<T>(Action<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Action cannot be null when registering.");
            }


            lock (_lockObject)
            {
                if (!_eventListeners.ContainsKey(typeof(T))) 
                {
                    _eventListeners[typeof(T)] = new List<Delegate>();
                }
                _eventListeners[typeof(T)].Add(handler);
            }
        }

        public void RemoveListener<T>(Action<T> handler) {

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null when unregistering");
            }

            lock (_lockObject)
            {
                if (_eventListeners.ContainsKey(typeof(T)))
                {
                    _eventListeners[typeof(T)].Remove(handler);
                }
            }
        }

        public void Publish<T>(T data)
        {
            List<Delegate> handlersToInvoke = null;

            lock (_lockObject)
            {
                if (_eventListeners.TryGetValue(typeof(T), out var listenersAsObject))
                {
                    handlersToInvoke = new List<Delegate>((List<Delegate>)listenersAsObject);
                }
            }


            if (handlersToInvoke != null)
            {
                foreach (var handler in handlersToInvoke)
                {
                    ((Action<T>)handler)?.Invoke(data);
                }
            }
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _eventListeners.Clear();
            }
        }
    }
}
