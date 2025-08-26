namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Defines a generic factory interface for creating objects of type <typeparamref name="T"/> using configuration or data of type <typeparamref name="Tparam"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    /// <typeparam name="Tparam">The type of data or configuration required to create the object.</typeparam>
    public interface IFactory<T, Tparam>
    {
        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/> using the provided data or configuration.
        /// </summary>
        /// <param name="data">The data or configuration used to create the object.</param>
        /// <returns>A new instance of type <typeparamref name="T"/>.</returns>
        T Create(Tparam data);
    }
}
