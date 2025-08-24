using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace GameEngine.Core.Services
{
    /// <summary>
    /// Provides a global service locator for registering, retrieving, and managing services by type.
    /// Enables dependency injection and service access throughout the application.
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Stores registered services mapped by their interface type.
        /// </summary>
        public Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance for the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service.</typeparam>
        /// <param name="service">The service instance to register.</param>
        /// <exception cref="ArgumentNullException">Thrown if service is null.</exception>
        public void Register<TInterface>(TInterface service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null when registering.");
            }
            _services[typeof(TInterface)] = service;
        }

        /// <summary>
        /// Checks if a service of the specified interface type is registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface type to check.</typeparam>
        /// <returns>True if the service is registered; otherwise, false.</returns>
        public bool HasService<TInterface>()
        {
            return _services.ContainsKey(typeof(TInterface));
        }

        /// <summary>
        /// Unregisters the service of the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service to unregister.</typeparam>
        public void UnregisterService<TInterface>()
        {
            if (HasService<TInterface>())
            {
                _services.Remove(typeof(TInterface));
            }
        }

        /// <summary>
        /// Retrieves the registered service instance for the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service.</typeparam>
        /// <returns>The registered service instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not registered.</exception>
        public TInterface GetService<TInterface>() where TInterface : class
        {
            if (!HasService<TInterface>())
            {
                throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} has not been registered.");
            }
            return (TInterface)_services[typeof(TInterface)];
        }

        /// <summary>
        /// Clears all registered services from the service locator.
        /// </summary>
        public void Clear()
        {
            _services.Clear();
        }
    }
}
