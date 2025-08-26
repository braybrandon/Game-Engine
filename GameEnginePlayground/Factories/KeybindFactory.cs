using GameEngine.Common.Config;
using GameEngine.Common.Enums;
using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using GameEngine.IO.Asset.models;
using System;
using System.Collections.Generic;

namespace GameEnginePlayground.Factories
{
    /// <summary>
    /// Factory for creating player input components with key bindings and input actions based on provided keybind data.
    /// Associates input actions with player entity and event manager for runtime input handling.
    /// </summary>
    public class KeybindFactory : IFactory<IComponent, Keybinds>
    {
        private readonly IEventManager _eventManager;
        private IEntity _playerEntity;
        private Dictionary<InputAction, Action<int, IWorld>> _inputActions;

        /// <summary>
        /// Initializes a new instance of the KeybindFactory with the specified player entity and event manager.
        /// </summary>
        /// <param name="playerEntity">The player entity to associate with input components.</param>
        /// <param name="eventManager">The event manager used to publish input-related events.</param>
        public KeybindFactory(IEntity playerEntity, IEventManager eventManager)
        {
            _playerEntity = playerEntity;
            _eventManager = eventManager;
            InitializeActions();
        }

        /// <summary>
        /// Creates a player input component using the provided keybind data, setting up key bindings and movement keys.
        /// </summary>
        /// <param name="data">The keybinds data containing attack and movement key mappings.</param>
        /// <returns>The configured PlayerInputComponent instance.</returns>
        public IComponent Create(Keybinds data)
        {
            ref var inputcomponent = ref _playerEntity.AddComponent(new PlayerInputComponent());
            foreach (var kva in data.Attack)
            {
                if (_inputActions.ContainsKey(kva.Value))
                {
                    inputcomponent.KeyBinds.Add(kva.Key, _inputActions[kva.Value]);
                }
            }
            foreach (var kva in data.Movement)
            {
                inputcomponent.MovementKeys.Add(kva.Value, kva.Key);
            }
            return inputcomponent;
        }

        /// <summary>
        /// Initializes the dictionary of input actions, associating input actions with their corresponding event logic.
        /// </summary>
        private void InitializeActions()
        {
            _inputActions = new Dictionary<InputAction, Action<int, IWorld>>()
            {
                { InputAction.Fireball, (entityId, world) => _eventManager.Publish(new ProjectileEvent(entityId, "fireball", world))}
            };
        }
    }
}
