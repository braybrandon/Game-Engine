using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    /// <summary>
    /// Non-generic interface for component pools, allowing the World to manage them polymorphically.
    /// </summary>
    public interface IComponentPool
    {
        /// <summary>
        /// Removes the component associated with the given entity ID from this pool.
        /// </summary>
        /// <param name="entityId">The ID of the entity whose component to remove.</param>
        void Remove(int entityId);

        /// <summary>
        /// Checks if this pool contains a component for the given entity ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity to check.</param>
        /// <returns>True if a component exists for the entity, false otherwise.</returns>
        bool Has(int entityId);

        /// <summary>
        /// Gets an enumerable collection of entity IDs that have a component in this pool.
        /// </summary>
        /// <returns>An enumerable of entity IDs.</returns>
        IEnumerable<int> GetEntityIds();
    }
}
