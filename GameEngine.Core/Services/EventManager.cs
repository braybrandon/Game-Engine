namespace GameEngine.Core.Services
{
    public static class EventManager
    {
        private static readonly object _lockObject = new object();

        private static Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

        public static bool HasEvent<T>()
        {
            lock (_lockObject)
            {
                return _eventListeners.ContainsKey(typeof(T));
            }
        }

        public static void AddListener<T>(Action<T> handler)
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

        public static void RemoveListener<T>(Action<T> handler) {

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

        public static void Publish<T>(T data)
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

        public static void Clear()
        {
            lock (_lockObject)
            {
                _eventListeners.Clear();
            }
        }
    }
}
