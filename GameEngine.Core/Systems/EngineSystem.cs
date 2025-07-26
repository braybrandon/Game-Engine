using GameEngine.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.Systems
{
    /// <summary>
    /// Base abstract class for all engine systems.
    /// This acts as your GameComponent base class in an ECS context.
    /// Systems contain logic and operate on components.
    /// </summary>
    public abstract class EngineSystem : IDisposable
    {

        /// <summary>
        /// Initializes the system.
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// Called once after content is loaded. Use for setup that depends on loaded content.
        /// </summary>
        public virtual void LoadContent() { }
        /// <summary>
        /// Called every fixed game update. Use for game logic, physics, AI, etc.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(World world) { }
        /// <summary>
        /// Called every frame for drawing operations.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch instance for drawing 2D sprites.</param>
        public virtual void Draw(SpriteBatch spriteBatch, World world) { }
        /// <summary>
        /// Called when the system is disposed. Use for cleaning up resources.
        /// </summary>
        public virtual void Dispose() { }
    }
}
