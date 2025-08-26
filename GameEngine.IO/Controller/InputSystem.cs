using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    /// <summary>
    /// System that processes player input by invoking bound actions for pressed keys on entities with PlayerInputComponent.
    /// Uses the provided input manager to query key states and execute corresponding actions.
    /// </summary>
    public class InputSystem : IUpdateSystem
    {
        private IInputManager _inputManager;

        /// <summary>
        /// Initializes a new instance of the InputSystem with the specified input manager.
        /// </summary>
        /// <param name="inputManager">The input manager used to query key states.</param>
        public InputSystem(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }

        /// <summary>
        /// Updates the input system, invoking actions for pressed keys on entities with PlayerInputComponent.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent>())
            {
                ref var inputComponent = ref entity.GetComponent<PlayerInputComponent>();
                foreach (Keys key in inputComponent.KeyBinds.Keys)
                {
                    if (_inputManager.IsKeyPressed(key))
                    {
                        inputComponent.KeyBinds[key].Invoke(entity.Id, world);
                    }
                }
            }
        }
    }
}
