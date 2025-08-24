using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Physics.Interfaces
{
    public interface IQuadTree
    {
        /// <summary>
        /// Removes all entities and child nodes from the quadtree.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inserts an entity with its bounding rectangle into the quadtree.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="worldBounds">The bounding rectangle of the entity in world space.</param>
        void Insert(IEntity entity, Rectangle worldBounds);

        /// <summary>
        /// Retrieves entities that intersect with the specified bounds.
        /// </summary>
        /// <param name="returnEntities">A list to populate with found entities.</param>
        /// <param name="bounds">The rectangle to check for intersections.</param>
        /// <returns>A list of entities that intersect with the bounds.</returns>
        List<IEntity> Retrieve(List<IEntity> returnEntities, Rectangle bounds);

        /// <summary>
        /// Removes an entity with its bounding rectangle from the quadtree.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <param name="worldBounds">The bounding rectangle of the entity in world space.</param>
        /// <returns>True if the entity was removed; otherwise, false.</returns>
        bool Remove(IEntity entity, Rectangle worldBounds);
    }
}
