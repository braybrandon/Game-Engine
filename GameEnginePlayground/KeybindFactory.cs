using Common.Config;
using Common.Enums;
using Common.Events;
using Common.Interfaces;
using GameEngine.IO.Asset.models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameEnginePlayground
{
    public class KeybindFactory : IKeybindFactory
    {
        private readonly IEventManager _eventManager;
        private readonly IInputManager _inputManager;
        private readonly IAssetManager _assetManager;
        private Dictionary<InputAction, Action> _inputActions;
        public KeybindFactory(IAssetManager assetManager, IInputManager inputManager, IEventManager eventManager) {
            _eventManager = eventManager;
            _inputManager = inputManager;
            _assetManager = assetManager;
            InitializeActions();
        }

        public void LoadContent()
        {
            var keybinds = _assetManager.Load<Keybinds>(FileNameConfig.Keybinds);
            foreach (var kva in keybinds.Attack)
            {
                if (_inputActions.ContainsKey(kva.Value))
                {
                    _inputManager.BindKeyDown(kva.Key, _inputActions[kva.Value]);
                }
                
            }
            foreach(var kva in keybinds.Movement)
            {
                _inputManager.BindMovementKeys(kva.Key, kva.Value);
            }
        }

        private void InitializeActions()
        {
            _inputActions = new Dictionary<InputAction, Action>()
            {
            };
        }
    }
}
