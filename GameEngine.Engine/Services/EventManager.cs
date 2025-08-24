using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace GameEngine.Core.Services
{
    /// <summary>
    /// Manages event listeners and event publishing for the game engine.
    /// Provides thread-safe registration, removal, and invocation of event handlers for different event types.
    /// </summary>
    public class EventManager : IEventManager
    {
        private readonly object _lockObject = new object();
        private Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Checks if there are any listeners registered for the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to check.</typeparam>
        /// <returns>True if listeners exist for the event type; otherwise, false.</returns>
        public bool HasEvent<T>()
        {
            lock (_lockObject)
            {
                return _eventListeners.ContainsKey(typeof(T));
            }
        }

        /// <summary>
        /// Registers a listener for the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to listen for.</typeparam>
        /// <param name="handler">The action to invoke when the event is published.</param>
        /// <exception cref="ArgumentNullException">Thrown if handler is null.</exception>
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

        /// <summary>
        /// Unregisters a listener for the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to stop listening for.</typeparam>
        /// <param name="handler">The action to remove from the event listeners.</param>
        /// <exception cref="ArgumentNullException">Thrown if handler is null.</exception>
        public void RemoveListener<T>(Action<T> handler)
        {
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

        /// <summary>
        /// Publishes an event to all registered listeners for the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to publish.</typeparam>
        /// <param name="data">The event data to pass to listeners.</param>
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

        /// <summary>
        /// Clears all registered event listeners from the event manager.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _eventListeners.Clear();
            }
        }
    }
}
