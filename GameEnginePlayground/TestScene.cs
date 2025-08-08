using Common.Config;
using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Services;
using GameEngine.Graphics.Animations;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Render;
using GameEngine.IO.Asset.models;
using GameEngine.IO.Controller;
using GameEngine.Physics.Motion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameEnginePlayground
{
    public class TestScene : IScene
    {
        private IWorld _world;

        private List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private List<IDrawSystem> _drawSystems = new List<IDrawSystem>();

        public TestScene()
        {
            _world = new World();
        }

        public IWorld GetWorld()
        {
            return _world;
        }

        public void RegisterUpdateSystem(IUpdateSystem system)
        {
            if (!_updateSystems.Contains(system))
            {
                _updateSystems.Add(system);
            }
        }

        public void UnregiserUpdateSystem(IUpdateSystem system)
        {
            _updateSystems.Remove(system);
        }

        public void RegisterDrawSystem(IDrawSystem system)
        {
            if (!_drawSystems.Contains(system))
            {
                _drawSystems.Add(system);
            }
        }

        public void UnregiserDrawSystem(IDrawSystem system)
        {
            _drawSystems.Remove(system);
        }

        public void Update()
        {
                foreach (IUpdateSystem system in _updateSystems)
                {
                    system.Update(_world);
                }
        }
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

        public void UnloadContent()
        {
            _updateSystems.Clear();
            _drawSystems.Clear();
        }
    }
}
