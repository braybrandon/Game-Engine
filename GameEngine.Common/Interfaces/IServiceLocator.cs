namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Provides an interface for a service locator, enabling registration, retrieval, and management of services by type.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Registers a service instance for the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service.</typeparam>
        /// <param name="service">The service instance to register.</param>
        void Register<TInterface>(TInterface service);

        /// <summary>
        /// Checks if a service of the specified interface type is registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface type to check.</typeparam>
        /// <returns>True if the service is registered; otherwise, false.</returns>
        bool HasService<TInterface>();

        /// <summary>
        /// Unregisters the service of the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service to unregister.</typeparam>
        void UnregisterService<TInterface>();

        /// <summary>
        /// Retrieves the registered service instance for the specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the service.</typeparam>
        /// <returns>The registered service instance.</returns>
        TInterface GetService<TInterface>() where TInterface : class;

        /// <summary>
        /// Clears all registered services from the service locator.
        /// </summary>
        void Clear();
    }
}
