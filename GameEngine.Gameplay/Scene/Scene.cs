using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameEngine.Gameplay.Scene
{
    /// <summary>
    /// Represents a game scene, managing its world, update systems, and draw systems.
    /// Provides methods for system registration, update, draw, and content unloading.
    /// </summary>
    public class Scene : IScene
    {
        private IWorld _world;
        private List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private List<IDrawSystem> _drawSystems = new List<IDrawSystem>();

        /// <summary>
        /// Initializes a new instance of the Scene class and creates its world.
        /// </summary>
        public Scene()
        {
            _world = new World();
        }

        /// <summary>
        /// Gets the world associated with this scene.
        /// </summary>
        /// <returns>The IWorld instance for this scene.</returns>
        public IWorld GetWorld()
        {
            return _world;
        }

        /// <summary>
        /// Registers an update system to be called during scene updates.
        /// </summary>
        /// <param name="system">The update system to register.</param>
        public void RegisterUpdateSystem(IUpdateSystem system)
        {
            if (!_updateSystems.Contains(system))
            {
                _updateSystems.Add(system);
            }
        }

        /// <summary>
        /// Unregisters an update system so it is no longer called during scene updates.
        /// </summary>
        /// <param name="system">The update system to unregister.</param>
        public void UnregiserUpdateSystem(IUpdateSystem system)
        {
            _updateSystems.Remove(system);
        }

        /// <summary>
        /// Registers a draw system to be called during scene drawing.
        /// </summary>
        /// <param name="system">The draw system to register.</param>
        public void RegisterDrawSystem(IDrawSystem system)
        {
            if (!_drawSystems.Contains(system))
            {
                _drawSystems.Add(system);
            }
        }

        /// <summary>
        /// Unregisters a draw system so it is no longer called during scene drawing.
        /// </summary>
        /// <param name="system">The draw system to unregister.</param>
        public void UnregiserDrawSystem(IDrawSystem system)
        {
            _drawSystems.Remove(system);
        }

        /// <summary>
        /// Updates all registered update systems and removes inactive entities from the world.
        /// </summary>
        public void Update()
        {
            foreach (IUpdateSystem system in _updateSystems)
            {
                system.Update(_world);
            }
            _world.RemoveInactiveEntities();
        }

        /// <summary>
        /// Draws all registered draw systems using the provided SpriteBatch and the scene's world.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for rendering.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                foreach (IDrawSystem system in _drawSystems)
                {
                    system.Draw(spriteBatch, _world);
                }
            }
        }

        /// <summary>
        /// Unloads all content by clearing registered update and draw systems.
        /// </summary>
        public void UnloadContent()
        {
            _updateSystems.Clear();
            _drawSystems.Clear();
        }
    }
}
