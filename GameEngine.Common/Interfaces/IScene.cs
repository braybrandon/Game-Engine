using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Defines the interface for a game scene, which manages update and draw systems, event systems, and world context.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Registers an update system to be called during scene updates.
        /// </summary>
        /// <param name="system">The update system to register.</param>
        void RegisterUpdateSystem(IUpdateSystem system);

        /// <summary>
        /// Unregisters an update system so it is no longer called during scene updates.
        /// </summary>
        /// <param name="system">The update system to unregister.</param>
        void UnregiserUpdateSystem(IUpdateSystem system);

        /// <summary>
        /// Registers an event system to handle events within the scene.
        /// </summary>
        /// <param name="system">The event system to register.</param>
        void RegisterEventSystem(IEventSystem system);

        /// <summary>
        /// Unregisters an event system so it is no longer called within the scene.
        /// </summary>
        /// <param name="system">The event system to unregister.</param>
        void UnregisterEventSystem(IEventSystem system);

        /// <summary>
        /// Gets the world associated with this scene.
        /// </summary>
        /// <returns>The world instance for this scene.</returns>
        IWorld GetWorld();

        /// <summary>
        /// Registers a draw system to be called during scene rendering.
        /// </summary>
        /// <param name="system">The draw system to register.</param>
        void RegisterDrawSystem(IDrawSystem system);

        /// <summary>
        /// Unregisters a draw system so it is no longer called during scene rendering.
        /// </summary>
        /// <param name="system">The draw system to unregister.</param>
        void UnregiserDrawSystem(IDrawSystem system);

        /// <summary>
        /// Updates all registered update systems and processes scene logic.
        /// </summary>
        void Update();

        /// <summary>
        /// Draws all registered draw systems using the provided SpriteBatch and the scene's world.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for rendering.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Unloads all content by clearing registered update and draw systems.
        /// </summary>
        void UnloadContent();
    }
}
