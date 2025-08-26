using GameEngine.Common.Enums;
using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Common.IO.Components
{
    /// <summary>
    /// Component that flags an entity as player-controlled and stores key bindings for input actions.
    /// Used to associate keyboard input mappings and actions with player entities for input handling and game logic.
    /// </summary>
    public struct PlayerInputComponent : IComponent
    {
        /// <summary>
        /// Gets the dictionary mapping keyboard keys to actions performed on the entity and world.
        /// </summary>
        public Dictionary<Keys, Action<int, IWorld>> KeyBinds { get; }

        /// <summary>
        /// Gets the dictionary mapping movement actions to keyboard keys.
        /// </summary>
        public Dictionary<InputAction, Keys> MovementKeys { get; }

        /// <summary>
        /// Initializes a new instance of the PlayerInputComponent, setting up empty key binding dictionaries.
        /// </summary>
        public PlayerInputComponent()
        {
            KeyBinds = new Dictionary<Keys, Action<int, IWorld>>();
            MovementKeys = new Dictionary<InputAction, Keys>();
        }
    }

}
